using HealthCare.Dtos;
using HealthCare.Models;
using HealthCare.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly RoleService _roleService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService, RoleService roleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleService = roleService;
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

            await _roleService.AssignRoleToUser(user, "User");

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

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = _tokenService.GenerateToken(user, authClaims);

            return Ok(new { token });
        }

        [HttpGet("user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Email,
                Roles = userRoles
            });
        }

        [HttpPost("assign-role")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(assignRoleDto.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _roleService.AssignRoleToUser(user, assignRoleDto.Role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("add-admin")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddAdmin([FromBody] AddAdminDto addAdminDto)
        {
            var user = new AppUser { UserName = addAdminDto.Email, Email = addAdminDto.Email };

            var result = await _userManager.CreateAsync(user, addAdminDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _roleService.AssignRoleToUser(user, "Admin");

            return Ok(user);
        }



        [HttpPost("remove-role")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto assignRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(assignRoleDto.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, assignRoleDto.Role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
