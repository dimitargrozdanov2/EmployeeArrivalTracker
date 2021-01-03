using Microsoft.AspNetCore.Builder;

namespace ReportingTool.Web.Infrastructure
{
    public static class ValidationExceptionHandlerMiddleWareExtensions
    {
        public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
          => builder.UseMiddleware<ValidationExceptionHandlerMiddleware>();
    }
}
