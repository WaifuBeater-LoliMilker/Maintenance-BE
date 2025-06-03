using MaintenanceSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        IGenericRepo _repo;
        public TestController(IGenericRepo repo)
        {
            _repo = repo;
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var result = await _repo.ProcedureToList<Lines, Factories, RefreshTokens>(
                       "spGetDataTest",
                       new string[] { },
                       new object[] { }
                   );
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
