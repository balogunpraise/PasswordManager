using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Dtos;
using PasswordManager.Api.Wrapper;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Core.Domain.Entities;
using System.Security.Claims;

namespace PasswordManager.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            return Ok( new ApiResponse<UserDto>(200, _mapper.Map<AppUser, UserDto>(user), "Succeeded"));
        }

        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();
            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();
            return Ok(new ApiResponse<UserDto>(200, new UserDto
            {
                DisplayName = user.FirstName + user.LastName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            }, "Succeeded"));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto userIn)
        {
            var user = _mapper.Map<RegisterDto, AppUser>(userIn);
            var duplicateUser = await _userManager.FindByEmailAsync(user.Email);
            if (duplicateUser != null) return BadRequest(new ApiResponse(400));
            var result = await _userManager.CreateAsync(user, userIn.PasswordHash);
            if (result.Succeeded)
            {
                return Ok(new UserDto
                {
                    DisplayName = user.FirstName +" "+ user.LastName,
                    Email = userIn.Email,
                    Token = "token"
                });
            }
            return BadRequest(new ApiResponse(400));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
