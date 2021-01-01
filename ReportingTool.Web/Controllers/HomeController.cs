using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReportingTool.Services;
using ReportingTool.Services.Contracts;
using ReportingTool.Web.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReportingTool.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceTokenService serviceTokenService;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IServiceTokenService serviceTokenService, IConfiguration configuration)
        {
            _logger = logger;
            this.serviceTokenService = serviceTokenService;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var exampleDate = new DateTime(2016, 3, 10);
            var callback = "http://localhost:51396/api/values";
            var z = await this.serviceTokenService.GetServiceToken(configuration["WebServiceUrl"], exampleDate, callback);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
