using Application.Services.Interfacess;
using Core.Entities;
using Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace DoorProject
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly IWorkContext _workContext;
        private readonly IRepository<Tag> _userTagRepository;

        public TokenManagerMiddleware(IWorkContext workContext,IRepository<Tag> userTagRepository)
        {
            _workContext = workContext;
            _userTagRepository = userTagRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
           
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                await next(context);
                return;
            }

            if (endpoint.DisplayName.Contains("Logout")|| endpoint.DisplayName.Contains("LeaveOffice"))
            {
                await next(context);
                return;
            }

            // Get current user
            var user = await _workContext.GetCurrentUserAsync();
            var tag = await _userTagRepository.GetByIdAsync(user.TagId);
            bool valid = tag.Status == Core.Entities.Enum.TagStatus.Active;
            if (!valid)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            if (tag.StartDate.HasValue && tag.ExpireDate.HasValue)
            {
                valid = tag.StartDate <= DateTime.Now && tag.ExpireDate >= DateTime.Now;
            }
            if (!valid)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            if (await _workContext.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
