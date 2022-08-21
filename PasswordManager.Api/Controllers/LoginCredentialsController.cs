using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Api.Security;
using PasswordManager.Api.Services;
using PasswordManager.Api.Wrapper;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;
using System.Net.Mime;
using System.Security.Claims;

namespace PasswordManager.Api.Controllers
{
    public class LoginCredentialsController : BaseApiController
    {
        private readonly IPasswordDetailsRepository _repo;
        private readonly EncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginCredentialsController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IDataProtector _dataProtector;

        public LoginCredentialsController(ILogger<LoginCredentialsController> logger,
            EncryptionService encryptionService, IPasswordDetailsRepository repo,
            IConfiguration config, IMapper mapper, UserManager<AppUser> userManager,
            IDataProtectionProvider dataProtectionProvider, DataProtectorString dataProtectorString)
        {
            _logger = logger;
            _repo = repo;
            _encryptionService = encryptionService;
            _configuration = config;
            _mapper = mapper;
            _userManager = userManager;
            _dataProtector = dataProtectionProvider.CreateProtector(dataProtectorString.PasswordProtectionString);
        }


        [HttpGet("getcredential/{id}")]
        [Authorize]
        public async Task<ActionResult> GetCredential([FromQuery] string id)
        {
            string AuthUserId = GetLoggedInUserId();
            var cred = await _repo.GetCredential(AuthUserId, Int32.Parse(id));
            if (cred != null)
            {
                var response = _mapper.Map<LoginCredential, LoginCredentialDto>(cred);
                response.Password = _dataProtector.Unprotect(response.Password);
                return Ok(new ApiResponse<LoginCredentialDto>(200, response, "Succeeded"));
            }
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("credentials")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<LoginCredentialDto>>> GetAllCredentials()
        {
            string AuthUserId = GetLoggedInUserId();
            var creds = await _repo.GetCredentials(AuthUserId);
            var response = _mapper.Map<IReadOnlyList<LoginCredential>, IReadOnlyList<LoginCredentialDto>>(creds);
            response.Select(x => _dataProtector.Unprotect(x.Password));
            return Ok(new ApiResponse<IReadOnlyList<LoginCredentialDto>>(200, response, "Succeeded"));
        }

        [HttpPost("addcredential")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLoginData(LoginCredentialDto credDto)
        {
            string AuthUserId = GetLoggedInUserId();
            credDto.Password = _dataProtector.Protect(credDto.Password);
            var loginData = _mapper.Map<LoginCredentialDto, LoginCredential>(credDto);
            loginData.UserId = AuthUserId;
            await _repo.AddNewCredentials(loginData);
            return NoContent();
        }

        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UpdateCredential(LoginCredentialDto input, string Id)
        {
            try
            {
                //string AuthUserId = GetLoggedInUserId();
                input.Password = _dataProtector.Protect(input.Password);
                var mappedInput = _mapper.Map<LoginCredentialDto, LoginCredential>(input);
                await _repo.UpdateCredentials(int.Parse(Id), mappedInput);
                return Ok();
            }
            catch
            {
                return BadRequest(new ApiResponse(400));
            }
     
        }

        private string GetLoggedInUserId()
        {
            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return  _userManager.FindByEmailAsync(email).Result.Id;
        }
    }
}
