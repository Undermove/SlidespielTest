using SlidespielApi.Endpoints.Videos;

namespace SlidespielApi.Services;

public interface IPowerPointParser
{
    Task<List<SlideVideosDto>> ExtractVideosAsync(string filePath);
}