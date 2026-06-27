using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeretanaBackend.Data;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthService authService;

        public AuthenticationController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterVM payload)
        {
            if (!ModelState.IsValid) return BadRequest("Please provide all required fields!");
            try
            {
                var result = await authService.RegisterAsync(payload);
                return Created(nameof(Register), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginVM payload)
        {
            if (!ModelState.IsValid) return BadRequest("Please provide all required fields!");

            var result = await authService.LoginAsync(payload);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVM payload)
        {
            var result = await authService.RefreshTokenAsync(payload);

            if (result is null) return BadRequest("Error while creating refresh token");

            return Ok(result);
        }
    }
}
