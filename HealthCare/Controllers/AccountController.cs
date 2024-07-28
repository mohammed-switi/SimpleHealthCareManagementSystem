    using HealthCare.Dtos;
    using HealthCare.Services;
    using Microsoft.AspNetCore.Mvc;
    
namespace HealthCare.Controllers
{


    
        [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly TokenService _tokenService;

            public AccountController(TokenService tokenService)
            {
                _tokenService = tokenService;
            }

            [HttpPost("login")]
            public IActionResult Login([FromBody] AppUserDTO user)
            {
                
            /*
             * TODO : Add Authentication Logic here 
             * I havent' implemented the authenticaiton logic yet!
             * Now you must do another vew model or DTO for the user contains the username and password
             * and implementing the logic for checking the user credentials
             * if successful return the token
             * otherwise return unauthroized
             */
                var token = _tokenService.GenerateToken(user);
                return Ok(new { token });
            }
        }
    }


