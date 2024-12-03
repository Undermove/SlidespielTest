namespace SlidespielApi.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<bool> FileExistsAsync(string filePath);
}