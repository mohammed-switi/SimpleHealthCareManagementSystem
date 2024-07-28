using HealthCare.Models;
using HealthCare.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HealthCare.Dtos;

namespace HealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
        {
            var doctors = await _doctorRepository.GetAllDoctors();
            var doctorsDTO = _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
            return Ok(doctorsDTO);
        }

        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctor(int id)
        {
            try
            {
                var doctor = await _doctorRepository.GetDoctorById(id);
                var doctorDTO = _mapper.Map<DoctorDTO>(doctor);
                return Ok(doctorDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Doctor
        [HttpPost]
        public async Task<ActionResult> CreateDoctor([FromBody] DoctorDTO doctorDTO)
        {
            var doctor = _mapper.Map<Doctor>(doctorDTO);
            await _doctorRepository.AddDoctor(doctor);
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.DoctorId }, doctorDTO);
        }

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDoctor(int id, [FromBody] DoctorDTO doctorDTO)
        {
            if (id != doctorDTO.DoctorId)
            {
                return BadRequest("Doctor ID mismatch");
            }

            try
            {
                var doctor = _mapper.Map<Doctor>(doctorDTO);
                await _doctorRepository.UpdateDoctor(doctor);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Doctor/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            try
            {
                await _doctorRepository.DeleteDoctor(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
