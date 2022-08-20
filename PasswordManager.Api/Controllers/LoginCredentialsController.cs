using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Api.Services;
using PasswordManager.Api.Wrapper;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;
using System.Net.Mime;

namespace PasswordManager.Api.Controllers
{
    public class LoginCredentialsController : BaseApiController
    {
        private readonly IPasswordDetailsRepository _repo;
        private readonly EncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginCredentialsController> _logger;
        private readonly IMapper _mapper;
        public LoginCredentialsController(ILogger<LoginCredentialsController> logger,
            EncryptionService encryptionService, IPasswordDetailsRepository repo,
            IConfiguration config, IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _encryptionService = encryptionService;
            _configuration = config;
            _mapper = mapper;
        }

        public string AuthUserId { get { return HttpContext.User.Identity.Name; } }

        [HttpGet("getcredential/{id}")]
        public async Task<ActionResult> GetCredential([FromQuery] string id)
        {
            var cred = await _repo.GetCredential(Int32.Parse(id));
            if (cred != null)
            {
                var response = _mapper.Map<LoginCredential, LoginCredentialDto>(cred);
                response.Password = _encryptionService.Decrypt(AuthUserId, cred.Password);
                return Ok(new ApiResponse<LoginCredentialDto>(200, response, "Succeeded"));
            }
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("credentials")]
        public async Task<ActionResult<IReadOnlyList<LoginCredentialsDto>>> GetAllCredentials()
        {
            var creds = await _repo.GetCredentials();
            var response = _mapper.Map<IReadOnlyList<LoginCredential>, IReadOnlyList<LoginCredentialDto>>(creds);
            response.Select(x => _encryptionService.Decrypt(AuthUserId, x.Password));
            return Ok(new ApiResponse<IReadOnlyList<LoginCredentialDto>>(200, response, "Succeeded"));
        }

        [HttpPost("addcredential")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLoginData(LoginCredentialsDto credDto)
        {
            credDto.Password = _encryptionService.Encrypt(AuthUserId, credDto.Password);
            credDto.UserId = Int32.Parse(AuthUserId);
            var loginData = _mapper.Map<LoginCredentialsDto, LoginCredential>(credDto);
            await _repo.AddNewCredentials(loginData);
            return CreatedAtAction(nameof(GetCredential), new { id = loginData.Id });
        }

        [HttpDelete("deletcredential/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCredential([FromQuery] string Id)
        {
            try
            {
                await _repo.RemoveCredentials(Int32.Parse(Id));
                return NoContent();
            }
            catch
            {
                return BadRequest(new ApiResponse(400));
            }
        }

        [HttpPut("update/{Id}")]
        public async Task<IActionResult> UpdateCredential(LoginCredentialDto input, string Id)
        {
            try
            {
                input.Password = _encryptionService.Encrypt(AuthUserId, input.Password);
                var mappedInput = _mapper.Map<LoginCredentialDto, LoginCredential>(input);
                await _repo.UpdateCredentials(int.Parse(Id), mappedInput);
                return Ok();
            }
            catch
            {
                return BadRequest(new ApiResponse(400));
            }
     
        }
    }
}
