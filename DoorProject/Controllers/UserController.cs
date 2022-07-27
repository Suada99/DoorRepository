using Application.Models.DTOs;
using Application.Services.Interfaces;
using Core.Entities.Enum;
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
        /// <response code="403">Forbidden, you are not allowed.</response>
        [HttpGet]
        [Route("GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetAllUsersResponse>> GetAllUsers([FromQuery] TagStatus? tagStatus)
        {
            var result = await _userService.GetAllUsers(tagStatus);
            if (result.Success)
            {
                return Ok(new GetAllUsersResponse
                {
                    Users = result.Data
                });
            }
            return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
        }

        /// <summary>
        /// Update user tag
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="200">If user tag is updated successfully </response>
        [HttpPut]
        [Route("UpdateTag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateTag([FromBody] UpdateTagDto updateTagDto)
        {
            if (ModelState.IsValid)
            {
                if (updateTagDto.TagStatus.ToString() == string.Empty)
                {
                    return BadRequest("Tag status can not be null or empty");
                }
                // Invoke UserService
                var result = await _userService.UpdateUserTag(updateTagDto.UserId, updateTagDto.TagStatus);
                if (result.Success)
                {
                    return Ok();
                }
                return StatusCode((int)result.CommandError.HttpCode, result.CommandError);
            }
            return BadRequest(ModelState);
        }
    }
}
