using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class CoursesController(IApiService apiService) : Controller
{
    private async Task LoadViewBagData(int? selectedCategoryId = null)
    {
        var cats = await apiService.GetAsync<List<CategoryViewModel>>("api/categories");
        var categories = cats.Data ?? new List<CategoryViewModel>();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        ViewBag.Levels = new SelectList(new[]
        {
            new { Value = "Beginner", Text = "Beginner" },
            new { Value = "Intermediate", Text = "Intermediate" },
            new { Value = "Advanced", Text = "Advanced" }
        }, "Value", "Text");
    }

    public async Task<IActionResult> Index(string? search, int? categoryId, string? level, bool? isPublished)
    {
        ViewData["Title"] = "Courses";
        ViewData["ActiveMenu"] = "Courses";
        ViewData["PageDescription"] = "Manage all courses";

        var url = "api/courses";
        var query = new List<string>();
        if (!string.IsNullOrEmpty(search)) query.Add($"searchTerm={search}");
        if (categoryId.HasValue) query.Add($"categoryId={categoryId}");
        if (!string.IsNullOrEmpty(level)) query.Add($"level={level}");
        if (query.Count > 0) url += "?" + string.Join("&", query);

        var result = await apiService.GetAsync<List<CourseViewModel>>(url);
        var model = result.IsSuccess && result.Data != null ? result.Data : new List<CourseViewModel>();

        await LoadViewBagData(categoryId);
        ViewBag.Search = search;
        ViewBag.CategoryId = categoryId;
        ViewBag.Level = level;
        ViewBag.IsPublished = isPublished;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Create Course";
        ViewData["ActiveMenu"] = "Courses";
        await LoadViewBagData();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadViewBagData(model.CategoryId);
            return View(model);
        }

        var result = await apiService.PostAsync<CourseViewModel>("api/courses", new
        {
            title = model.Title,
            description = model.Description,
            price = model.Price,
            level = model.Level,
            categoryId = model.CategoryId
        });

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error creating course");
            await LoadViewBagData(model.CategoryId);
            return View(model);
        }

        TempData["Success"] = "Course created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Course";
        ViewData["ActiveMenu"] = "Courses";

        var result = await apiService.GetAsync<CourseViewModel>($"api/courses/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        var model = new UpdateCourseViewModel
        {
            Title = result.Data.Title,
            Description = result.Data.Description,
            Price = result.Data.Price,
            Level = result.Data.Level,
            CategoryId = result.Data.CategoryId
        };

        ViewBag.CourseId = id;
        await LoadViewBagData(result.Data.CategoryId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateCourseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CourseId = id;
            await LoadViewBagData(model.CategoryId);
            return View(model);
        }

        var result = await apiService.PutAsync<CourseViewModel>($"api/courses/{id}", new
        {
            title = model.Title,
            description = model.Description,
            price = model.Price,
            level = model.Level,
            categoryId = model.CategoryId
        });

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error updating course");
            ViewBag.CourseId = id;
            await LoadViewBagData(model.CategoryId);
            return View(model);
        }

        TempData["Success"] = "Course updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Course Details";
        ViewData["ActiveMenu"] = "Courses";

        var result = await apiService.GetAsync<CourseViewModel>($"api/courses/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Delete Course";
        ViewData["ActiveMenu"] = "Courses";

        var result = await apiService.GetAsync<CourseViewModel>($"api/courses/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await apiService.DeleteAsync<object>($"api/courses/{id}");
        if (!result.IsSuccess)
        {
            TempData["Error"] = "Error deleting course";
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["Success"] = "Course deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Publish(int id)
    {
        await apiService.PostAsync<object>($"api/courses/{id}/publish", null);
        TempData["Success"] = "Course published!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UploadThumbnail(int id)
    {
        ViewData["Title"] = "Upload Thumbnail";
        ViewData["ActiveMenu"] = "Courses";
        ViewBag.CourseId = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadThumbnail(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Please select a file");
            ViewBag.CourseId = id;
            return View();
        }

        var result = await apiService.PostFileAsync<object>($"api/courses/{id}/thumbnail", file, "file");
        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error uploading thumbnail");
            ViewBag.CourseId = id;
            return View();
        }

        TempData["Success"] = "Thumbnail uploaded successfully!";
        return RedirectToAction(nameof(Index));
    }
}
