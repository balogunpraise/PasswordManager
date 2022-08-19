using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Api.Services;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Api.Controllers
{
    public class LoginCredentialsController : BaseApiController
    {
        private readonly IPasswordDetailsRepository _repo;
        private readonly EncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginCredentialsController> _logger;
        public LoginCredentialsController(ILogger<LoginCredentialsController> logger, 
            EncryptionService encryptionService, IPasswordDetailsRepository repo,
            IConfiguration config)
        {
            _logger = logger;
            _repo = repo;
            _encryptionService = encryptionService;
            _configuration = config;
        }

        public string AuthUserId { get { return HttpContext.User.Identity.Name; } }

        [HttpGet("logincredentials")]
        public async Task<ActionResult<List<LoginCredResponseDto>>> GetCredentials()
        {
            
        }

        public async Task<IActionResult> AddLoginData(LoginCredentialsDto credDto, string Id)
        {
            var loginData = new LoginCredential
            {
                WebsiteName = credDto.WebsiteName,
                Email = credDto.Email,
                Password = _encryptionService.Encrypt(AuthUserId, credDto.Password),
                WebAddress = credDto.WebAddress,
                Note = credDto.Note,
                UserId = Int32.Parse(AuthUserId)
            };
            try
            {
                await _repo.AddNewCredentials(loginData);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return Ok();
        }
    }
}
