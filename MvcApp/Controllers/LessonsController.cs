using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class LessonsController(IApiService apiService) : Controller
{
    private async Task LoadCourses(int? selectedCourseId = null)
    {
        var courses = await apiService.GetAsync<List<CourseViewModel>>("api/courses");
        var list = courses.Data ?? new List<CourseViewModel>();
        ViewBag.Courses = new SelectList(list, "Id", "Title", selectedCourseId);
    }

    public async Task<IActionResult> Index(int? courseId)
    {
        ViewData["Title"] = "Lessons";
        ViewData["ActiveMenu"] = "Lessons";
        ViewData["PageDescription"] = "Manage course lessons";

        await LoadCourses(courseId);

        if (!courseId.HasValue)
            return View(new List<LessonViewModel>());

        var result = await apiService.GetAsync<List<LessonViewModel>>($"api/lessons/course/{courseId}");
        var model = result.IsSuccess && result.Data != null ? result.Data : new List<LessonViewModel>();

        ViewBag.SelectedCourseId = courseId;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? courseId)
    {
        ViewData["Title"] = "Create Lesson";
        ViewData["ActiveMenu"] = "Lessons";
        await LoadCourses(courseId);
        return View(new CreateLessonViewModel { CourseId = courseId ?? 0 });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLessonViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCourses(model.CourseId);
            return View(model);
        }

        var result = await apiService.PostAsync<LessonViewModel>("api/lessons", new
        {
            title = model.Title,
            content = model.Content,
            order = model.Order,
            courseId = model.CourseId
        });

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error creating lesson");
            await LoadCourses(model.CourseId);
            return View(model);
        }

        TempData["Success"] = "Lesson created successfully!";
        return RedirectToAction(nameof(Index), new { courseId = model.CourseId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Lesson";
        ViewData["ActiveMenu"] = "Lessons";

        var result = await apiService.GetAsync<LessonViewModel>($"api/lessons/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        var model = new UpdateLessonViewModel
        {
            Title = result.Data.Title,
            Content = result.Data.Content,
            Order = result.Data.Order
        };

        ViewBag.LessonId = id;
        ViewBag.CourseId = result.Data.CourseId;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateLessonViewModel model, int courseId)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.LessonId = id;
            ViewBag.CourseId = courseId;
            return View(model);
        }

        var result = await apiService.PutAsync<LessonViewModel>($"api/lessons/{id}", new
        {
            title = model.Title,
            content = model.Content,
            order = model.Order
        });

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.Error ?? "Error updating lesson");
            ViewBag.LessonId = id;
            ViewBag.CourseId = courseId;
            return View(model);
        }

        TempData["Success"] = "Lesson updated successfully!";
        return RedirectToAction(nameof(Index), new { courseId });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Lesson Details";
        ViewData["ActiveMenu"] = "Lessons";

        var result = await apiService.GetAsync<LessonViewModel>($"api/lessons/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Delete Lesson";
        ViewData["ActiveMenu"] = "Lessons";

        var result = await apiService.GetAsync<LessonViewModel>($"api/lessons/{id}");
        if (!result.IsSuccess || result.Data is null)
            return NotFound();

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, int courseId)
    {
        var result = await apiService.DeleteAsync<object>($"api/lessons/{id}");
        if (!result.IsSuccess)
        {
            TempData["Error"] = "Error deleting lesson";
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["Success"] = "Lesson deleted successfully!";
        return RedirectToAction(nameof(Index), new { courseId });
    }
}
