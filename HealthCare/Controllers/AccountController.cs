using HealthCare.Dtos;
using HealthCare.Models;
using HealthCare.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var user = new AppUser { UserName = registerDto.Email, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid login attempt.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid login attempt.");
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }

    }
}
