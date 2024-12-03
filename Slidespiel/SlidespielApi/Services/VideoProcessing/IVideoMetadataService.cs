namespace SlidespielApi.Services.VideoProcessing;

public interface IVideoMetadataService
{
    Task<string> GetVideoDurationAsync(string videoPath);
}