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
        private readonly IUserService _userService;

        public DoorController(IWorkContext workContext, IUserService userService)
        {
            _workContext = workContext;
            _userService = userService;
        }

        /// <summary>
        /// Enter office
        /// </summary>
        /// <returns>Success true followed with a message if you entered in your office</returns>
        [HttpPost]
        [Route("EnterOffice")]
        public async Task<IActionResult> EnterOffice()
        {
            var result = await _workContext.GetCurrentUserAsync();
            if (result == null)
            {
                return BadRequest("There is no logged in user.");
            }
            result.InOffice = true;
            await _userService.UpdateUserAsync(result);

            return Ok($"Welcome {(await _workContext.GetCurrentUserAsync()).UserName}, you just entered in your office.");
        }

        /// <summary>
        /// Leave office
        /// </summary>
        /// <returns>Success true followed with a message if you left your office</returns>
        [HttpPost]
        [Route("LeaveOffice")]
        public async Task<IActionResult> LeaveOffice()
        {
            var result = await _workContext.GetCurrentUserAsync();
            if (result == null)
            {
                return BadRequest("There is no logged in user.");
            }
            result.InOffice = false;
            await _userService.UpdateUserAsync(result);

            return Ok($"Goodbye {(await _workContext.GetCurrentUserAsync()).UserName}, you just left your office.");
        }

    }
}
