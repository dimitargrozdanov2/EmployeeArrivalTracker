using Microsoft.AspNetCore.Mvc;

namespace ReportingTool.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult PageNotFound()
        {
            return this.View();
        }

        public IActionResult TokenExpired()
        {
            return this.View();
        }

    }
}