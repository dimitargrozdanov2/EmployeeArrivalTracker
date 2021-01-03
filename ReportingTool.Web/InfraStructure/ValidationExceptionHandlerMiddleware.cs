using Microsoft.AspNetCore.Http;
using ReportingTool.Data.Exceptions;
using System;
using System.Threading.Tasks;

namespace ReportingTool.Web.Infrastructure
{
    public class ValidationExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ValidationExceptionHandlerMiddleware(RequestDelegate next)
            => this.next = next;

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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                //logger.Log(ex.)
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
