using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Api.Controllers
{
    public class LoginCredentialsController : BaseApiController
    {
        private readonly IPasswordDetailsRepository _repo;
        private readonly ILogger<LoginCredentialsController> _logger;
        public LoginCredentialsController(ILogger<LoginCredentialsController> logger, IPasswordDetailsRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        public async Task<IActionResult> CreateCredential([FromBody]LoginCredentialsDto credDto)
        {
            _logger.LogInformation("CreateCredential Executing");
            if (ModelState.IsValid)
            {
                var cred = new LoginCredential
                {
                    WebsiteName = credDto.WebsiteName,
                    WebAddress = credDto.WebAddress,
                    Email = credDto.Email,
                    Password = credDto.Password
                };
                await _repo.AddNewCredentials(cred);
                return Ok();
            }
            return BadRequest();
        }
    }
}
