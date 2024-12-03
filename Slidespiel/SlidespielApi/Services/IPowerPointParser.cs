using SlidespielApi.Endpoints.Videos.Models;

namespace SlidespielApi.Services;

public interface IPowerPointParser
{
    Task<List<SlideVideosDto>> ExtractVideosAsync(string filePath);
}