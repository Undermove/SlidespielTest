namespace SlidespielApi.Endpoints;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
}