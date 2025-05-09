using MaintenanceSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private IGenericRepo _repo;

        public StationsController(IGenericRepo repo)
        {
            _repo = repo;
            // Constructor logic if needed
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "line-id")] int lineId)
        {
            var stations = new List<Stations>();
            if (lineId > 0)
            {
                stations = await _repo.FindByExpression<Stations>(x => x.LineId == lineId);
            }
            else
            {
                stations = await _repo.GetAll<Stations>();
            }
            return Ok(stations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var station = await _repo.GetById<Stations>(id);
            if (station == null) return NotFound();
            return Ok(station);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Stations station)
        {
            if (station == null) return BadRequest("Station cannot be null.");
            // Optional: check for duplicate by ID or unique field
            var existingStation = await _repo.GetById<Stations>(station.Id);
            if (existingStation != null)
            {
                return Conflict($"Station with ID {station.Id} already exists.");
            }
            var inserted = await _repo.Insert(station);
            return CreatedAtAction(nameof(GetById), new { id = inserted.Id }, inserted);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Stations station)
        {
            if (station == null || station.Id != id)
                return BadRequest("Station ID mismatch.");
            var existing = await _repo.GetById<Stations>(id);
            if (existing == null) return NotFound();
            var updated = await _repo.Update(station);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetById<Stations>(id);
            if (existing == null) return NotFound();
            var deleted = await _repo.Delete(existing);
            return Ok(deleted);
        }
    }
}