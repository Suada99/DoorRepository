using Application.Models.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

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
        /// <response code="409">If requested user name, email or role already exists </response>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Register([FromBody] UserRegistrationDto user)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDto user)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Logout()
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
