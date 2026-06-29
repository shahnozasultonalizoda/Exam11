using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

public class AccountController(IApiService apiService) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            return RedirectToAction("Index", "Dashboard");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await apiService.PostAsync<LoginResponseViewModel>("api/auth/login", new
        {
            email = model.Email,
            password = model.Password
        });

        if (!result.IsSuccess || result.Data is null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        if (result.Data.Role != "Admin")
        {
            ModelState.AddModelError("", "Access denied. Admin role required.");
            return View(model);
        }

        HttpContext.Session.SetString("Token", result.Data.Token);
        HttpContext.Session.SetString("Role", result.Data.Role);
        HttpContext.Session.SetString("FullName", result.Data.FullName);
        HttpContext.Session.SetString("Email", result.Data.Email);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
