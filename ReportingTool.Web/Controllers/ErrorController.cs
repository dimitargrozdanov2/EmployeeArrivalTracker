using Microsoft.AspNetCore.Mvc;

namespace ReportingTool.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult PageNotFound() => this.View();

        public IActionResult TokenExpired() => this.View();

    }
}