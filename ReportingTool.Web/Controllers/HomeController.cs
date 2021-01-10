using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingTool.Data.Models;
using ReportingTool.Services.Contracts;
using ReportingTool.Web.Services;
using ReportingTool.Web.Utils;
using System;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index(string sortOrder, string employeeIdFilter, string whenFilter, string currentFilter,
 int? pageNumber)
        {
            if (await tokenService.TokenAlreadyExistsAsync(Request))
            {
                return await ArrivalsFromDatabase(sortOrder, employeeIdFilter, whenFilter, currentFilter, pageNumber);
            }

            var exampleDate = new DateTime(2016, 3, 10);
            bool success = false;

            var token = await this.tokenService.GetServiceToken(exampleDate);
            var f = HttpContext.Session.Keys.ToList();

            this.tokenService.TryGetTokenFromSession(token.Token);
            if (token != null && !string.IsNullOrEmpty(token.Token))
            {
                await this.tokenService.SavesTokenAsync(token);
                success = true;
            }
            
            if (!success)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            var name = HttpContext.Session.GetString("ServiceToken");
            return await ArrivalsFromDatabase(sortOrder, employeeIdFilter, whenFilter, currentFilter, pageNumber);
        }

        public async Task<IActionResult> ReceiveArrivalInfoFromService()
        {
            await tokenService.ReadTokenAsync(Request);
            var arrivals = await tokenService.CollectArrivals(Request);
            var name = HttpContext.Session.GetString("ServiceToken");
            await arrivalService.AddRangeAsync(arrivals);
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private async Task<IActionResult> ArrivalsFromDatabase(string sortOrder, string employeeIdFilter, string whenFilter, string currentFilter,
 int? pageNumber)
        {
            ViewData["EmployeeIdParm"] = sortOrder == "EmployeeId" ? "employeeid_desc" : "EmployeeId";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["EmployeeIdFilter"] = employeeIdFilter;
            ViewData["WhenFilter"] = whenFilter;

            var arrivals = this.arrivalService.GetAll();

            if (!string.IsNullOrEmpty(employeeIdFilter))
            {
                arrivals = arrivals.Where(s => s.EmployeeId.Equals(int.Parse(employeeIdFilter)));
                pageNumber = 1;

            }
            else
            {
                employeeIdFilter = currentFilter;
            }

            if (!string.IsNullOrEmpty(whenFilter))
            {
                var tokenExpireDateTime = DateTime.Parse(whenFilter);
                var y = DateTime.Parse(whenFilter);
                arrivals = arrivals.Where(s => s.When.CompareTo(tokenExpireDateTime) <= -1);
                pageNumber = 1;
            }

            else
            {
                whenFilter = currentFilter;
            }

            arrivals = sortOrder switch
            {
                "EmployeeId" => arrivals.OrderBy(a => a.EmployeeId),
                "employeeid_desc" => arrivals.OrderByDescending(a => a.EmployeeId),
                "Date" => arrivals.OrderBy(a => a.When),
                "date_desc" => arrivals.OrderByDescending(a => a.When),
                _ => arrivals,
            };
            int pageSize = 20;
            return View(await PaginatedList<Arrival>.CreateAsync(arrivals.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await arrivals.AsNoTracking().ToListAsync());
        }

    }
}
