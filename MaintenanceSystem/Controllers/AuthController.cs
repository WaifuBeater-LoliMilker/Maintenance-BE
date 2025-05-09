using MaintenanceSystem.Auth;
using MaintenanceSystem.Models;
using MaintenanceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IGenericRepo _repo;

        public AuthController(IAuthService authService, IGenericRepo genericRepo)
        {
            _authService = authService;
            _repo = genericRepo;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequest req)
        {
            var (res, refreshToken) = await _authService.Authenticate(req);
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(15)
            });
            return Ok(res);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var tokenEntity = await _repo.FindModel<RefreshTokens>(x => x.Token == refreshToken);
                if (tokenEntity != null)
                {
                    await _repo.Delete<RefreshTokens>(tokenEntity);
                }
            }

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok(new { message = "See ya later, aligator!" });
        }
    }
}