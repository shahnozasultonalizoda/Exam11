using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MvcApp.Models.Common;

namespace MvcApp.Services;

public class ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IApiService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient("BackendApi");
        var token = httpContextAccessor.HttpContext?.Session.GetString("Token");
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<ApiResponse<T?>> GetAsync<T>(string url)
    {
        var client = CreateClient();
        var response = await client.GetAsync(url);
        return await ReadResponse<T>(response);
    }

    public async Task<ApiResponse<T?>> PostAsync<T>(string url, object? body)
    {
        var client = CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        return await ReadResponse<T>(response);
    }

    public async Task<ApiResponse<T?>> PutAsync<T>(string url, object? body)
    {
        var client = CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await client.PutAsync(url, content);
        return await ReadResponse<T>(response);
    }

    public async Task<ApiResponse<T?>> PatchAsync<T>(string url, object? body = null)
    {
        var client = CreateClient();
        var content = body is null ? null : new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(url, content);
        return await ReadResponse<T>(response);
    }

    public async Task<ApiResponse<T?>> DeleteAsync<T>(string url)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync(url);
        return await ReadResponse<T>(response);
    }

    public async Task<ApiResponse<T?>> PostFileAsync<T>(string url, IFormFile file, string fileFieldName = "file")
    {
        var client = CreateClient();
        using var form = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        form.Add(fileContent, fileFieldName, file.FileName);
        var response = await client.PostAsync(url, form);
        return await ReadResponse<T>(response);
    }

    private static async Task<ApiResponse<T?>> ReadResponse<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var data = JsonSerializer.Deserialize<T>(json, JsonOptions);
                return new ApiResponse<T?> { IsSuccess = true, Data = data, StatusCode = (int)response.StatusCode };
            }
            catch
            {
                return new ApiResponse<T?> { IsSuccess = true, StatusCode = (int)response.StatusCode };
            }
        }
        return new ApiResponse<T?> { IsSuccess = false, Error = json, StatusCode = (int)response.StatusCode };
    }
}
