using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using TechStore.Api.Models;
using TechStore.Api.Services;

namespace TechStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        
        [HttpPost("register")]
        public async Task<ActionResult<User>>Register(UserDto request)
        {
           var user=await authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Username already exists.");
            }

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result= await authService.LoginAsync(request);
            if(result is null)
            {
                return BadRequest("Invalid username or password!");
            }
            return Ok(result);
        }
        [HttpPost("refresh-tokens")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null)
            {
                return BadRequest("Invalid or expired refresh token!");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthentificatedOnlyEndPoint()
        {
            return Ok("You are authenticated");
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("You are an admin!");
        }

    
    }

   
}
