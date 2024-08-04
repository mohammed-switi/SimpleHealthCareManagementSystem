    using HealthCare.Dtos;
using HealthCare.Models;
using HealthCare.Repositories;
using HealthCare.Services;
    using Microsoft.AspNetCore.Mvc;
    
namespace HealthCare.Controllers
{


    
        [ApiController]
        [Route("api/[controller]")]
        public class AuthenticationController : ControllerBase
        {
            private readonly TokenService _tokenService;
        private readonly IAppUserRepository _userRepository;

            public AuthenticationController(TokenService tokenService, IAppUserRepository userRepository)
            {
                _tokenService = tokenService;
             _userRepository = userRepository;
            }


        [HttpPost("signup")]
        public async Task <IActionResult> Signup([FromBody] SignupDto signupDto)
        {
            _tokenService.CreatePasswordHash(signupDto.password, out byte[] passwordHash, out byte[] passwordSalt);
            AppUser user = new AppUser()
            {
                firstName = signupDto.FirstName,
                lastName = signupDto.LastName,
                UserEmail = signupDto.Email,
                PasswordHashed = passwordHash,
                PasswordSalt = passwordSalt,
                phone = signupDto.Phone

            };

           await _userRepository.AddAppUser(user);


            return Ok(user);


        }


            [HttpPost("login")]
            public IActionResult Login([FromBody] LoginDto user)
            {

            /*
             * TODO : Add Authentication Logic here 
             * I havent' implemented the authenticaiton logic yet!
             * Now you must do another vew model or DTO for the user contains the username and password
             * and implementing the logic for checking the user credentials
             * if successful return the token
             * otherwise return unauthroized
             */
            //var token = _tokenService.GenerateToken(user);
            //return Ok(new { token });

            return Ok();
            }
        }
    }


