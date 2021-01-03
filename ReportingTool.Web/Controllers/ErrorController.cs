using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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