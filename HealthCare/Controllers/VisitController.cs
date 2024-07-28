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
    public class VisitController : ControllerBase
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IMapper _mapper;

        public VisitController(IVisitRepository visitRepository, IMapper mapper)
        {
            _visitRepository = visitRepository;
            _mapper = mapper;
        }

        // GET: api/Visit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitDTO>>> GetVisits()
        {
            var visits = await _visitRepository.GetAllVisits();
            var visitsDTO = _mapper.Map<IEnumerable<VisitDTO>>(visits);
            return Ok(visitsDTO);
        }

        // GET: api/Visit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VisitDTO>> GetVisit(int id)
        {
            try
            {
                var visit = await _visitRepository.GetVisitById(id);
                var visitDTO = _mapper.Map<VisitDTO>(visit);
                return Ok(visitDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Visit
        [HttpPost]
        public async Task<ActionResult> CreateVisit([FromBody] VisitDTO visitDTO)
        {
            var visit = _mapper.Map<Visit>(visitDTO);
            await _visitRepository.AddVisit(visit);
            return CreatedAtAction(nameof(GetVisit), new { id = visit.VisitId }, visitDTO);
        }

        // PUT: api/Visit/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVisit(int id, [FromBody] VisitDTO visitDTO)
        {
            if (id != visitDTO.VisitId)
            {
                return BadRequest("Visit ID mismatch");
            }

            try
            {
                var visit = _mapper.Map<Visit>(visitDTO);
                await _visitRepository.UpdateVisit(visit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Visit/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVisit(int id)
        {
            try
            {
                await _visitRepository.DeleteVisit(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
