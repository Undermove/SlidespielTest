using DocumentFormat.OpenXml.Packaging;
using SlidespielApi.Endpoints.Videos.Models;
using SlidespielApi.Services.VideoProcessing;

namespace SlidespielApi.Services;

public class PowerPointParser(IVideoDownloadService videoDownloadService) : IPowerPointParser
{
    public async Task<List<SlideVideosDto>> ExtractVideosAsync(string filePath)
    {
        var result = new List<SlideVideosDto>();
        var downloadFolder = Path.Combine(Directory.GetCurrentDirectory(), "DownloadedVideos");

        using var document = PresentationDocument.Open(filePath, false);

        if (document.PresentationPart is null)
        {
            return result;
        }

        var slideNumber = 1;
        foreach (var slidePart in document.PresentationPart.SlideParts)
        {
            var videos = new List<VideoInfoDto>();

            foreach (var relationship in slidePart.HyperlinkRelationships)
            {
                var videoUrl = relationship.Uri.OriginalString;
                var localFilePath = await videoDownloadService.DownloadVideoAsync(videoUrl, downloadFolder);

                if (!IsVideoFile(localFilePath))
                {
                    continue;
                }
                
                videos.Add(new VideoInfoDto
                {
                    FileName = Path.GetFileName(localFilePath),
                    FilePath = localFilePath,
                    Duration = await GetVideoDurationAsync(localFilePath)
                });
            }

            if (videos.Count > 0)
            {
                result.Add(new SlideVideosDto
                {
                    SlideNumber = slideNumber,
                    Videos = videos
                });
            }

            slideNumber++;
        }

        return result;
    }
    
    private static bool IsVideoFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();
        return extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".wmv";
    }

    // Заглушка для получения длительности видео
    private async Task<string> GetVideoDurationAsync(string videoPath)
    {
        await Task.Delay(10);
        return "00:02:30";
    }
}