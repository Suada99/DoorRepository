using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Core.Services
{
    public partial class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IJWTTokenService _jWTTokenService;

        public WorkContext(IHttpContextAccessor httpContextAccessor, IUserService userService, IJWTTokenService jWTTokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _jWTTokenService = jWTTokenService;
        }

        public virtual async Task<User> GetCurrentUserAsync()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            return await _userService.GetUserByEmailAsync(email);
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveTokenAsync(GetCurrentTokenAsync());

        public async Task DeactivateCurrentTokenAsync()
            => await DeactivateTokenAsync(GetCurrentTokenAsync());

        public async Task<bool> IsActiveTokenAsync(string token)
        {
            var jwtToken = await _jWTTokenService.GetJWTTokenByTokenAsync(token);
            if (jwtToken != null)
            {
                if (jwtToken.Deactivated)
                    return false;

                return true;
            }
            return false;
        }

        public async Task DeactivateTokenAsync(string token)
        {
            var jwtToken = await _jWTTokenService.GetJWTTokenByTokenAsync(token);
            jwtToken.Deactivated = true;

            await _jWTTokenService.UpdateJWTTokenAsync(jwtToken);
        }

        private string GetCurrentTokenAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["Authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        public virtual bool IsAdmin { get; set; }
    }
}
