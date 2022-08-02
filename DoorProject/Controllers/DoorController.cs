using Application.Services.Interfaces;
using Application.Services.Interfacess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorProject.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DoorController : ControllerBase
    {
        private readonly IWorkContext _workContext;
        private readonly IDoorService _doorService;

        public DoorController(IWorkContext workContext, IDoorService doorService)
        {
            _workContext = workContext;
            _doorService = doorService;
        }

        /// <summary>
        /// Enter office
        /// </summary>
        /// <returns>Success true followed with a message if you entered in your office</returns>
        [HttpPost]
        [Route("EnterOffice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> EnterOffice()
        {
            var loggedUser = await _workContext.GetCurrentUserAsync();
            if (loggedUser == null)
            {
                return BadRequest("There is no logged in user.");
            }
            // Invoke DoorService
            var result = await _doorService.EnterOfficeAsync(loggedUser);
            if (result.Success)
            {
                return Ok($"Welcome {result.Data.UserName}, you just entered in your office.");
            }
            return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
        }

        /// <summary>
        /// Leave office
        /// </summary>
        /// <returns>Success true followed with a message if you left your office</returns>
        [HttpPost]
        [Route("LeaveOffice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> LeaveOffice()
        {
            var loggedUser = await _workContext.GetCurrentUserAsync();
            if (loggedUser == null)
            {
                return BadRequest("There is no logged in user.");
            }
            // Invoke DoorService
            var result = await _doorService.LeaveOfficeAsync(loggedUser);
            if (result.Success)
            {
                return Ok($"Goodbye {result.Data.UserName}, you just left your office.");
            }
            return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
        }
    }
}
