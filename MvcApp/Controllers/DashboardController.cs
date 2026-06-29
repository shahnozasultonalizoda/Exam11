using Microsoft.AspNetCore.Mvc;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class DashboardController(IApiService apiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        ViewData["ActiveMenu"] = "Dashboard";
        ViewData["PageDescription"] = "Overview of the Online Courses Platform";

        var result = await apiService.GetAsync<DashboardViewModel>("api/dashboard/summary");
        var model = result.IsSuccess && result.Data != null ? result.Data : new DashboardViewModel();

        return View(model);
    }
}
