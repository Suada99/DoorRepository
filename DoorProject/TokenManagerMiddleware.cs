using Core.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace DoorProject
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly IWorkContext _workContext;

        public TokenManagerMiddleware(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                await next(context);
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
