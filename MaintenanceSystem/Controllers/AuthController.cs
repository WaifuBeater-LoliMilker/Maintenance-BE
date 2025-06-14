﻿using MaintenanceSystem.Auth;
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
            try
            {
                if (HttpContext.Items["User"] != null) return Accepted();
                var (res, refreshToken) = await _authService.Authenticate(req);
                var existedToken = await _repo.FindModel<RefreshTokens>(t => t.UserId == res.UserId);
                var newRefreshToken = new RefreshTokens()
                {
                    UserId = res.UserId,
                    Token = refreshToken,
                    CreatedDate = DateTime.UtcNow,
                    ExpireDate = DateTime.UtcNow.AddDays(15),
                    IsRevoked = false
                };
                if (existedToken != null)
                {
                    newRefreshToken.Id = existedToken.Id;
                    await _repo.Update<RefreshTokens>(newRefreshToken);
                }
                else await _repo.Insert<RefreshTokens>(newRefreshToken);
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(15)
                });
                return Ok(res);
            }
            catch (NullReferenceException)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok(new { message = "See ya later, aligator!" });
        }
        [HttpPost("refresh")]
        [AllowAnonymous]
        [SkipJWTMiddleware]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (HttpContext.Items["User"] != null) return Accepted(); // still valid => do nothing
                var refreshToken = Request.Cookies["refreshToken"];
                if (refreshToken == null) return Forbid();
                var existedToken = await _repo.FindModel<RefreshTokens>(t => t.Token == refreshToken);
                if (existedToken == null) return Forbid();
                if (existedToken.ExpireDate < DateTime.UtcNow) return Forbid();
                else
                {
                    var user = await _repo.GetById<Users>(existedToken.UserId);
                    var newSessionToken = _authService.GenerateAccessToken(user!);
                    return Ok(new { access_token = newSessionToken });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        
        }
        [HttpPost("role")]
        public IActionResult Role()
        {
            if (HttpContext.Items["User"] is not Users user) return BadRequest();
            return Ok(new { role = user!.Role == 0 ? "managers" : "users" });
        }
    }
}