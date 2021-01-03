using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReportingTool.Data.Exceptions;
using System;
using System.Threading.Tasks;

namespace ReportingTool.Web.Infrastructure
{
    public class ValidationExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ValidationExceptionHandlerMiddleware> logger;

        public ValidationExceptionHandlerMiddleware(RequestDelegate next, ILogger<ValidationExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);

                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/Error/PageNotFound");
                }
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
                logger.LogError(exception, exception.Message);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = string.Empty;

            switch (exception)
            {
                case TokenExpiryException _:
                    context.Response.Redirect($"/Error/TokenExpired");
                    break;
                case NotFoundException _:
                    context.Response.Redirect($"/Error/PageNotFound");
                    break;
            }

            if (String.IsNullOrEmpty(result))
            {
                context.Response.Redirect($"/Error/PageNotFound");

            }
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);
        }
    }
}
