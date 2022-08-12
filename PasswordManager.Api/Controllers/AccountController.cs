using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();
            return Ok(new UserDto
            {
                DisplayName = user.FirstName + user.LastName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto userIn)
        {
            var user = new AppUser
            {
                FirstName = userIn.FirstName,
                LastName = userIn.LastName,
                Email = userIn.Email,
            };

            var duplicateUser = await _userManager.FindByEmailAsync(user.Email);
            if (duplicateUser != null) return BadRequest();
            var result = await _userManager.CreateAsync(user, userIn.Password);
            if (result.Succeeded)
            {
                return Ok(new UserDto
                {
                    DisplayName = user.FirstName + user.LastName,
                    Email = userIn.Email,
                    Token = "token"
                });
            }
            return BadRequest();
        }
    }
}
