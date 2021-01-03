using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReportingTool.Services.Contracts;
using ReportingTool.Web.Models;
using ReportingTool.Web.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReportingTool.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        private readonly IArrivalService arrivalService;
        public HomeController(ILogger<HomeController> logger, ITokenService tokenService, IConfiguration configuration, IArrivalService arrivalService)
        {
            _logger = logger;
            this.tokenService = tokenService;
            this.configuration = configuration;
            this.arrivalService = arrivalService;
        }

        public async Task<IActionResult> Index()
        {
            if (await tokenService.TokenAlreadyExistsAsync(Request))
            {
                return ArrivalsFromDatabase();
            }

            var exampleDate = new DateTime(2016, 3, 10);
            var callback = Url.Action("ReceiveArrivalInfoFromService", "Home", null, Request.Scheme);
            bool success = false;

            var token = await this.tokenService.GetServiceToken(exampleDate, callback);

            if (!string.IsNullOrEmpty(token.Token))
            {
                await this.tokenService.SavesTokenAsync(token);
                success = true;
            }
            
            if (!success)
            {
                return View("Error");
            }

            return ArrivalsFromDatabase();
        }
        public async Task<IActionResult> ReceiveArrivalInfoFromService()
        {
            await tokenService.ReadTokenAsync(Request);
            var arrivals = await tokenService.CollectArrivals(Request);
            await arrivalService.AddRangeAsync(arrivals);
            return Ok();
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

        private IActionResult ArrivalsFromDatabase()
        {
            //TO DO :get arrivals and return view with arrivals sorted and paginated.
            return null;
        }

    }
}
