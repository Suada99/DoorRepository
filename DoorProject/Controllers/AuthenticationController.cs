using Application.Models.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.RegisterUserAsync(user);
                if (result.Success)
                {
                    return Ok();
                }
                return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
            }
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.LogInUserAsync(user);

                if (result.Success)
                {
                    return Ok(result.Data);
                }
                return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [Authorize]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticationService.LogOutUserAsync();
            if (result.Success)
            {
                return Ok();
            }
            return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
        }

    }
}
