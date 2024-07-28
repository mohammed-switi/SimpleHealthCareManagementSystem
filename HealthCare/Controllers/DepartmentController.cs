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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            var departments = await _departmentRepository.GetAllDepartments();
            var departmentsDTO = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
            return Ok(departmentsDTO);
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentById(id);
                var departmentDTO = _mapper.Map<DepartmentDTO>(department);
                return Ok(departmentDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Department
        [HttpPost]
        public async Task<ActionResult> CreateDepartment([FromBody] DepartmentDTO departmentDTO)
        {
            if (departmentDTO == null)
            {
                return BadRequest("Department is null.");
            }

            var department = _mapper.Map<Department>(departmentDTO);
            await _departmentRepository.AddDepartment(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = department.DpId }, departmentDTO);
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] DepartmentDTO departmentDTO)
        {
            if (id != departmentDTO.DpId)
            {
                return BadRequest("Department ID mismatch.");
            }

            try
            {
                var department = _mapper.Map<Department>(departmentDTO);
                await _departmentRepository.UpdateDepartment(department);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                await _departmentRepository.DeleteDepartment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
