using EX.Api.Handlers;
using Microsoft.AspNetCore.Builder;

namespace EX.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAppException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
