using Application.Results;

namespace Application.DTOs.CourseDTOs;

public class CourseFilterDto : PagedRequest
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public string? Level { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
