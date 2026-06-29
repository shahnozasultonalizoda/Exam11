using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class UsersController(IApiService apiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Users";
        ViewData["ActiveMenu"] = "Users";
        ViewData["PageDescription"] = "Manage platform users";

        var result = await apiService.GetAsync<List<UserViewModel>>("api/users");
        var model = result.IsSuccess && result.Data != null ? result.Data : new List<UserViewModel>();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "User Details";
        ViewData["ActiveMenu"] = "Users";

        var result = await apiService.GetAsync<UserViewModel>($"api/users/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(int id)
    {
        ViewData["Title"] = "Edit User Role";
        ViewData["ActiveMenu"] = "Users";

        var result = await apiService.GetAsync<UserViewModel>($"api/users/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        ViewBag.Roles = new SelectList(new[] { "Admin", "Instructor", "Student" }, result.Data.Role);

        return View(new UpdateRoleViewModel
        {
            Id = result.Data.Id,
            FullName = result.Data.FullName,
            Email = result.Data.Email,
            Role = result.Data.Role
        });
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(int id, UpdateRoleViewModel model)
    {
        var result = await apiService.PutAsync<object>($"api/users/{id}/role", new { role = model.Role });

        if (!result.IsSuccess)
        {
            TempData["Error"] = "Error updating role";
            return RedirectToAction(nameof(EditRole), new { id });
        }

        TempData["Success"] = "Role updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await apiService.DeleteAsync<object>($"api/users/{id}");
        if (!result.IsSuccess)
            TempData["Error"] = "Error deleting user";
        else
            TempData["Success"] = "User deleted successfully!";

        return RedirectToAction(nameof(Index));
    }
}
