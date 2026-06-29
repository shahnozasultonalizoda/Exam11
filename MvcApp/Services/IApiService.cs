using MvcApp.Models.Common;

namespace MvcApp.Services;

public interface IApiService
{
    Task<ApiResponse<T?>> GetAsync<T>(string url);
    Task<ApiResponse<T?>> PostAsync<T>(string url, object? body);
    Task<ApiResponse<T?>> PutAsync<T>(string url, object? body);
    Task<ApiResponse<T?>> PatchAsync<T>(string url, object? body = null);
    Task<ApiResponse<T?>> DeleteAsync<T>(string url);
    Task<ApiResponse<T?>> PostFileAsync<T>(string url, IFormFile file, string fileFieldName = "file");
}
