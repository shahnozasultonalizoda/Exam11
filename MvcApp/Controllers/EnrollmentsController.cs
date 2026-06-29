using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApp.Filters;
using MvcApp.Models;
using MvcApp.Services;

namespace MvcApp.Controllers;

[JwtSessionAuthorize]
public class EnrollmentsController(IApiService apiService) : Controller
{
    public async Task<IActionResult> Index(int? studentId)
    {
        ViewData["Title"] = "Enrollments";
        ViewData["ActiveMenu"] = "Enrollments";
        ViewData["PageDescription"] = "Manage student enrollments";

        var students = await apiService.GetAsync<List<UserViewModel>>("api/users");
        var studentList = students.Data?.Where(u => u.Role == "Student").ToList() ?? new List<UserViewModel>();
        ViewBag.Students = new SelectList(studentList, "Id", "FullName", studentId);
        ViewBag.SelectedStudentId = studentId;

        if (!studentId.HasValue)
            return View(new List<EnrollmentViewModel>());

        var result = await apiService.GetAsync<List<EnrollmentViewModel>>($"api/enrollments/my");
        var model = result.IsSuccess && result.Data != null ? result.Data : new List<EnrollmentViewModel>();
        return View(model);
    }
}
