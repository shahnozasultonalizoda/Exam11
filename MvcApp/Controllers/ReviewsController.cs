using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class ReviewsController(IApiService apiService) : Controller
{
    public async Task<IActionResult> Index(int? courseId)
    {
        ViewData["Title"] = "Reviews";
        ViewData["ActiveMenu"] = "Reviews";
        ViewData["PageDescription"] = "Manage course reviews";

        var courses = await apiService.GetAsync<List<CourseViewModel>>("api/courses");
        var courseList = courses.Data ?? new List<CourseViewModel>();
        ViewBag.Courses = new SelectList(courseList, "Id", "Title", courseId);
        ViewBag.SelectedCourseId = courseId;

        if (!courseId.HasValue)
            return View(new List<ReviewViewModel>());

        var result = await apiService.GetAsync<List<ReviewViewModel>>($"api/courses/{courseId}/reviews");
        var model = result.IsSuccess && result.Data != null ? result.Data : new List<ReviewViewModel>();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int courseId)
    {
        var result = await apiService.DeleteAsync<object>($"api/courses/{courseId}/reviews/{id}");
        if (!result.IsSuccess)
            TempData["Error"] = "Error deleting review";
        else
            TempData["Success"] = "Review deleted successfully!";

        return RedirectToAction(nameof(Index), new { courseId });
    }
}
