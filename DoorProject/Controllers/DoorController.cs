using Core.Services;
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
        private readonly IWorkContext workContext;

        public DoorController(IWorkContext workContext)
        {
            this.workContext = workContext;
        }
        [HttpGet]
        [Route("OpenDoor")]
        public async Task<IActionResult> OpenDoor()
        {
            return Ok($"Welcome {(await workContext.GetCurrentUserAsync()).Email}");
        }
    }
}
