namespace Application.Interfaces.Services;

public interface IFileService
{
     Task<string> SaveFileAsync(IFormFile file, string folder);
    void DeleteFile(string path);
    bool IsValidImage(IFormFile file);
}
