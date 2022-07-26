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

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">If user is registered successfully </response>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                // Invoke AuthenticationService
                var result = await _authenticationService.RegisterUserAsync(user);
                if (result.Success)
                {
                    return Ok();
                }
                return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Log in with an exisiting user
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">If user is logged in successfully </response>
        /// <response code="401">If authentication isn't done </response>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                // Invoke AuthenticationService
                var result = await _authenticationService.LogInUserAsync(user);

                if (result.Success)
                {
                    return Ok(result.Data);
                }
                return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Logs out of system
        /// </summary>
        /// <response code="200">If user is logged out successfully </response>
        /// <response code="401">If authentication isn't done </response>
        [Authorize]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Invoke AuthenticationService
            var result = await _authenticationService.LogOutUserAsync();
            if (result.Success)
            {
                return Ok();
            }
            return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
        }
    }
}
