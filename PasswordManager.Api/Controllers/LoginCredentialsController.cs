using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Api.Services;
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
        [ProducesResponseType(typeof(LoginCredentialsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginCredentialDto>> GetCredential([FromQuery]string id)
        {
            var cred = await _repo.GetCredential(Int32.Parse(id));
            if(cred != null)
            {
                var response = _mapper.Map<LoginCredential, LoginCredentialsDto>(cred);
                response.Password = _encryptionService.Decrypt(AuthUserId, cred.Password);
                return Ok(response);
            }
            return BadRequest();
        }

        public async Task<ActionResult<IReadOnlyList<LoginCredentialsDto>>> GetAllCredentials()
        {
            var creds = await _repo.GetCredentials();
            var response = _mapper.Map<IReadOnlyList<LoginCredential>, IReadOnlyList<LoginCredentialDto>>(creds);
            response.Select(x => _encryptionService.Decrypt(AuthUserId, x.Password));
            return Ok(response);
        }

        [HttpPost("addcredential")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLoginData(LoginCredentialsDto credDto)
        {
            var loginData = _mapper.Map<LoginCredentialsDto, LoginCredential>(credDto);
            loginData.UserId = Int32.Parse(AuthUserId);
            await _repo.AddNewCredentials(loginData);
            return CreatedAtAction(nameof(GetCredential), new { id = loginData.Id });
        }

        [HttpDelete("deletcredential/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCredential([FromQuery]string Id)
        {
            await _repo.RemoveCredentials(Int32.Parse(Id));
            return Ok(Id);
        }
    }
}
