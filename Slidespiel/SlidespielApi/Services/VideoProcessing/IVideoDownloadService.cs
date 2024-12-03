namespace SlidespielApi.Services.VideoProcessing;

public interface IVideoDownloadService
{
    Task<string> DownloadVideoAsync(string videoUrl, string destinationFolder);
}