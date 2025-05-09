using MaintenanceSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private IGenericRepo _repo;

        public ZonesController(IGenericRepo repo)
        {
            _repo = repo;
            // Constructor logic if needed
        }

        // GET: api/zones
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "factory-id")] int factoryId)
        {
            var zones = new List<Zones>();
            if (factoryId > 0)
            {
                zones = await _repo.FindByExpression<Zones>(x => x.FactoryId == factoryId);
            }
            else
            {
                zones = await _repo.GetAll<Zones>();
            }
            return Ok(zones);
        }

        // GET: api/zones/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var zone = await _repo.GetById<Zones>(id);
            if (zone == null) return NotFound();
            return Ok(zone);
        }

        // POST: api/zones
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Zones zone)
        {
            if (zone == null) return BadRequest("Zone cannot be null.");
            // Optional: check for duplicate by ID or unique field
            var existingZone = await _repo.GetById<Zones>(zone.Id);
            if (existingZone != null)
            {
                return Conflict($"Zone with ID {zone.Id} already exists.");
            }
            var inserted = await _repo.Insert(zone);
            return CreatedAtAction(nameof(GetById), new { id = inserted.Id }, inserted);
        }

        // PUT: api/zones/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Zones zone)
        {
            if (zone == null || zone.Id != id)
                return BadRequest("Zone ID mismatch.");
            var existing = await _repo.GetById<Zones>(id);
            if (existing == null) return NotFound($"Zone with ID {id} not found.");
            var updated = await _repo.Update(zone);
            return Ok(updated);
        }

        // DELETE: api/zones/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetById<Zones>(id);
            if (existing == null) return NotFound($"Zone with ID {id} not found.");
            var deleted = await _repo.Delete<Zones>(existing);
            return Ok(deleted);
        }
    }
}