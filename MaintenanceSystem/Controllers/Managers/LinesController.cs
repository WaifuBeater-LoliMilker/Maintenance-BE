using MaintenanceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers.Managers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Managers")]
    public class LinesController : ControllerBase
    {
        private IGenericRepo _repo;

        public LinesController(IGenericRepo repo)
        {
            _repo = repo;
            // Constructor logic if needed
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "zone-id")] int zoneId)
        {
            var lines = new List<Lines>();
            if (zoneId > 0)
            {
                lines = await _repo.FindByExpression<Lines>(x => x.ZoneId == zoneId);
            }
            else
            {
                lines = await _repo.GetAll<Lines>();
            }
            return Ok(lines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var line = await _repo.GetById<Lines>(id);
            if (line == null) return NotFound();
            return Ok(line);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lines line)
        {
            if (line == null) return BadRequest("Line cannot be null.");
            // Optional: check for duplicate by ID or unique field
            var existingLine = await _repo.GetById<Lines>(line.Id);
            if (existingLine != null)
            {
                return Conflict($"Line with ID {line.Id} already exists.");
            }
            var inserted = await _repo.Insert(line);
            return CreatedAtAction(nameof(GetById), new { id = inserted.Id }, inserted);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Lines line)
        {
            if (line == null || line.Id != id)
                return BadRequest("Line ID mismatch.");
            var existing = await _repo.GetById<Lines>(id);
            if (existing == null) return NotFound($"Line with ID {id} not found.");
            var updated = await _repo.Update(line);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var line = await _repo.GetById<Lines>(id);
            if (line == null) return NotFound($"Line with ID {id} not found.");
            var deleted = await _repo.Delete(line);
            if (!deleted) return BadRequest("Failed to delete the line.");
            return NoContent();
        }
    }
}