using HealthCare.Models;
using HealthCare.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HealthCare.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace HealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AppUserController : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IMapper _mapper;

        public AppUserController(IAppUserRepository appUserRepository, IMapper mapper)
        {
            _appUserRepository = appUserRepository;
            _mapper = mapper;
        }

        // GET: api/AppUser
        [HttpGet]
        [Authorize(Policy ="AdminOnly")]
        public async Task<ActionResult<IEnumerable<AppUserDTO>>> GetAppUsers()
        {
            var appUsers = await _appUserRepository.GetAllAppUsers();
            var appUsersDTO = _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            return Ok(appUsersDTO);
        }

        // GET: api/AppUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserDTO>> GetAppUser(int id)
        {
            try
            {
                var appUser = await _appUserRepository.GetAppUserById(id);
                var appUserDTO = _mapper.Map<AppUserDTO>(appUser);
                return Ok(appUserDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/AppUser
        [HttpPost]
        public async Task<ActionResult> CreateAppUser([FromBody] AppUserDTO appUserDTO)
        {
            var appUser = _mapper.Map<AppUser>(appUserDTO);
            await _appUserRepository.AddAppUser(appUser);
            return CreatedAtAction(nameof(GetAppUser), new { id = appUser.Id }, appUserDTO);
        }

        // PUT: api/AppUser/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAppUser(int id, [FromBody] AppUserDTO appUserDTO)
        {
            if (id != appUserDTO.UserId)
            {
                return BadRequest("AppUser ID mismatch");
            }

            try
            {
                var appUser = _mapper.Map<AppUser>(appUserDTO);
                await _appUserRepository.UpdateAppUser(appUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/AppUser/5
        [HttpDelete("{id}")]
        [Authorize(Policy ="AdminOnly")]
        public async Task<ActionResult> DeleteAppUser(int id)
        {
            try
            {
                await _appUserRepository.DeleteAppUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
