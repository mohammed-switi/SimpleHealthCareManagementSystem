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
    public class TestController : ControllerBase
    {
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;

        public TestController(ITestRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _mapper = mapper;
        }

        // GET: api/Test
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDTO>>> GetTests()
        {
            var tests = await _testRepository.GetAllTests();
            var testsDTO = _mapper.Map<IEnumerable<TestDTO>>(tests);
            return Ok(testsDTO);
        }

        // GET: api/Test/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDTO>> GetTest(int id)
        {
            try
            {
                var test = await _testRepository.GetTestById(id);
                var testDTO = _mapper.Map<TestDTO>(test);
                return Ok(testDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Test
        [HttpPost]
        public async Task<ActionResult> CreateTest([FromBody] TestDTO testDTO)
        {
            var test = _mapper.Map<Test>(testDTO);
            await _testRepository.AddTest(test);
            return CreatedAtAction(nameof(GetTest), new { id = test.TestId }, testDTO);
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTest(int id, [FromBody] TestDTO testDTO)
        {
            if (id != testDTO.TestId)
            {
                return BadRequest("Test ID mismatch");
            }

            try
            {
                var test = _mapper.Map<Test>(testDTO);
                await _testRepository.UpdateTest(test);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Test/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTest(int id)
        {
            try
            {
                await _testRepository.DeleteTest(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
