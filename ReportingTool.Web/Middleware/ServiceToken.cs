using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ReportingTool.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ServiceToken
    {
        private readonly RequestDelegate _next;

        public ServiceToken(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // Add Token To Context If Found.
            string token = httpContext.Session.GetString("ServiceToken");

            // Check Something Was Found.
            if (!string.IsNullOrEmpty(token))
            {
                // Add The Custom Token Here.
                httpContext.Request.Headers.TryAdd("ServiceToken", token);
            }

            // Next.
            return _next(httpContext);
        }
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ServiceTokenExtensions
    {
        public static IApplicationBuilder UseServiceToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServiceToken>();
        }
    }
}
