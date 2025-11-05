using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EX.Api.Filters
{
    public class AddAuthHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo?.DeclaringType == null)
                return;

            var isAuthorized = (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any()) //this excludes controllers with AllowAnonymous attribute in case base controller has Authorize attribute
                               || (!context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizationFailure>().Any()
                                   && context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()); // this excludes methods with AllowAnonymous attribute

            if (!isAuthorized)
                return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                }
            };
        }
    }
}
