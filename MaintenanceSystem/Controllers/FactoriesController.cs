using MaintenanceSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoriesController : ControllerBase
    {
        private IGenericRepo _repo;

        public FactoriesController(IGenericRepo repo)
        {
            _repo = repo;
            // Constructor logic if needed
        }

        // GET: api/factories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var factories = await _repo.GetAll<Factories>();
            return Ok(factories);
        }

        // GET: api/factories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var factory = await _repo.GetById<Factories>(id);
            if (factory == null) return NotFound();
            return Ok(factory);
        }

        // POST: api/factories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Factories factory)
        {
            if (factory == null) return BadRequest("Factory cannot be null.");

            // Optional: check for duplicate by ID or unique field
            var existingFactory = await _repo.GetById<Factories>(factory.Id);
            if (existingFactory != null)
            {
                return Conflict($"Factory with ID {factory.Id} already exists.");
            }

            var inserted = await _repo.Insert(factory);
            return CreatedAtAction(nameof(GetById), new { id = inserted.Id }, inserted);
        }

        // PUT: api/factories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Factories factory)
        {
            if (factory == null || factory.Id != id)
                return BadRequest("Factory ID mismatch.");

            var existing = await _repo.GetById<Factories>(id);
            if (existing == null) return NotFound($"Factory with ID {id} not found.");

            var updated = await _repo.Update(factory);
            return Ok(updated);
        }

        // DELETE: api/factories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var factory = await _repo.GetById<Factories>(id);
            if (factory == null) return NotFound($"Factory with ID {id} not found.");

            await _repo.Delete(factory);
            return NoContent();
        }
    }
}