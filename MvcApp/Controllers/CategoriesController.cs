using Microsoft.AspNetCore.Mvc;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class CategoriesController(IApiService apiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Categories";
        ViewData["ActiveMenu"] = "Categories";
        ViewData["PageDescription"] = "Manage course categories";

        var result = await apiService.GetAsync<List<CategoryViewModel>>("api/categories");
        var model = result.IsSuccess && result.Data != null ? result.Data : new List<CategoryViewModel>();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Category";
        ViewData["ActiveMenu"] = "Categories";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await apiService.PostAsync<CategoryViewModel>("api/categories", new
        {
            name = model.Name,
            description = model.Description
        });

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error creating category");
            return View(model);
        }

        TempData["Success"] = "Category created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Category";
        ViewData["ActiveMenu"] = "Categories";

        var result = await apiService.GetAsync<CategoryViewModel>($"api/categories/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        var model = new CreateCategoryViewModel
        {
            Name = result.Data.Name,
            Description = result.Data.Description
        };
        ViewBag.CategoryId = id;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CategoryId = id;
            return View(model);
        }

        var result = await apiService.PutAsync<CategoryViewModel>($"api/categories/{id}", new
        {
            name = model.Name,
            description = model.Description
        });

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error updating category");
            ViewBag.CategoryId = id;
            return View(model);
        }

        TempData["Success"] = "Category updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Category Details";
        ViewData["ActiveMenu"] = "Categories";

        var result = await apiService.GetAsync<CategoryViewModel>($"api/categories/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Delete Category";
        ViewData["ActiveMenu"] = "Categories";

        var result = await apiService.GetAsync<CategoryViewModel>($"api/categories/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await apiService.DeleteAsync<object>($"api/categories/{id}");
        if (!result.IsSuccess)
        {
            TempData["Error"] = "Error deleting category";
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["Success"] = "Category deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
