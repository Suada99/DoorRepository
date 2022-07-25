using Core.Entities;
using Core.Services;
using DoorProject.Models.DTOs;
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

        [HttpGet]
        [Route("OpenDoor")]
        public async Task<IActionResult> OpenDoor()
        {
            var result =await _workContext.GetCurrentUserAsync();
            if (result != null)
            {
                result.InOffice = true;
                await _userService.UpdateUserAsync(result);
            }
            return Ok($"Welcome {(await _workContext.GetCurrentUserAsync()).Email}");
        }


        //[HttpPost]
        //[Route("AddRole")]
        //public async Task<IActionResult> AddRole(User user)
        //{
        //    List<UserRoles> userRoles = new();
        //    var result =await _userService.GetUserByEmailAsync(user.Email);
        //    if (result != null)
        //    {
        //        if (result.InOffice)
        //        {
        //        }
        //    }
        //    return Ok($"Welcome {(await _workContext.GetCurrentUserAsync()).Email}");
        //}
    }
}
