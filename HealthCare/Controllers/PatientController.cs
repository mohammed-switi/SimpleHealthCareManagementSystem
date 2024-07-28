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
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientController(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            var patients = await _patientRepository.GetAllPatients();
            var patientsDTO = _mapper.Map<IEnumerable<PatientDTO>>(patients);
            return Ok(patientsDTO);
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            try
            {
                var patient = await _patientRepository.GetPatientById(id);
                var patientDTO = _mapper.Map<PatientDTO>(patient);
                return Ok(patientDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Patient
        [HttpPost]
        public async Task<ActionResult> CreatePatient([FromBody] PatientDTO patientDTO)
        {
            var patient = _mapper.Map<Patient>(patientDTO);
            await _patientRepository.AddPatient(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.PtId }, patientDTO);
        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, [FromBody] PatientDTO patientDTO)
        {
            if (id != patientDTO.PtId)
            {
                return BadRequest("Patient ID mismatch");
            }

            try
            {
                var patient = _mapper.Map<Patient>(patientDTO);
                await _patientRepository.UpdatePatient(patient);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            try
            {
                await _patientRepository.DeletePatient(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
