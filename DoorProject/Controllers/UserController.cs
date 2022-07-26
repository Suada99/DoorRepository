using Application.Models.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorProject.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        /// <response code="200">If all users are returned successfully </response>
        /// <response code="401">If authentication isn't done </response>
        /// <response code="403">If you are forbidden </response>
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
        }
    }
}
