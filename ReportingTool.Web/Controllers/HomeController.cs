using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingTool.Services.Contracts;
using ReportingTool.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingTool.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenService tokenService;
        private readonly IArrivalService arrivalService;
        public HomeController(ITokenService tokenService, IArrivalService arrivalService)
        {
            this.tokenService = tokenService;
            this.arrivalService = arrivalService;
        }

        public async Task<IActionResult> Index(string sortOrder, string employeeIdFilter, string whenFilter)
        {
            if (await tokenService.TokenAlreadyExistsAsync(Request))
            {
                return await ArrivalsFromDatabase(sortOrder, employeeIdFilter, whenFilter);
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
                return RedirectToAction("PageNotFound", "Error");
            }

            return await ArrivalsFromDatabase(sortOrder, employeeIdFilter, whenFilter);
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

        private async Task<IActionResult> ArrivalsFromDatabase(string sortOrder, string employeeIdFilter, string whenFilter)
        {
            ViewData["EmployeeIdParm"] = sortOrder == "EmployeeId" ? "employeeid_desc" : "EmployeeId";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["EmployeeIdFilter"] = employeeIdFilter;
            ViewData["WhenFilter"] = whenFilter;

            var arrivals = this.arrivalService.GetAll();

            if (!string.IsNullOrEmpty(employeeIdFilter))
            {
                arrivals = arrivals.Where(s => s.EmployeeId.Equals(int.Parse(employeeIdFilter)));
            }

            if (!string.IsNullOrEmpty(whenFilter))
            {
                arrivals = arrivals.Where(s => s.When.Contains(whenFilter));
            }
            arrivals = sortOrder switch
            {
                "EmployeeId" => arrivals.OrderBy(a => a.EmployeeId),
                "employeeid_desc" => arrivals.OrderByDescending(a => a.EmployeeId),
                "Date" => arrivals.OrderBy(a => a.When),
                "date_desc" => arrivals.OrderByDescending(a => a.When),
                _ => arrivals,
            };
            return View(await arrivals.AsNoTracking().ToListAsync());
        }

    }
}
