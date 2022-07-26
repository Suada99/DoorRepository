﻿using Application.Services.Interfaces;
using Application.Services.Interfacess;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Application.Services
{
    public partial class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userService;
        private readonly IJWTTokenService _jWTTokenService;

        public WorkContext(IHttpContextAccessor httpContextAccessor, UserManager<User> userService, IJWTTokenService jWTTokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _jWTTokenService = jWTTokenService;
        }

        public virtual async Task<User> GetCurrentUserAsync()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            return await _userService.FindByEmailAsync(email);
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

    }
}
