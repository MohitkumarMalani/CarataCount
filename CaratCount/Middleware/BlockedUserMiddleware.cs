using System.Threading.Tasks;
using CaratCount.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CaratCount.Middleware
{
    public class BlockedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {

            var blockedRoutes = new[] { "/dashboard", "/profile", "/client", "/diamond-packet" };

            var requestPath = context.Request.Path;

            if (Array.Exists(blockedRoutes, route => requestPath.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase)))
               {
                var user = await userManager.GetUserAsync(context.User);

                if (user != null && user.IsBlocked)
                {
                    context.Response.Redirect("/account/blocked");
                    return;
                }
            }

            await _next(context);
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BlockedUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseBlockedUserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlockedUserMiddleware>();
        }
    }
}
