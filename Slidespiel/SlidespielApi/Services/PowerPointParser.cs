using DocumentFormat.OpenXml.Packaging;
using SlidespielApi.Endpoints.Videos.Models;

namespace SlidespielApi.Services;

public class PowerPointParser : IPowerPointParser
{
    public async Task<List<SlideVideosDto>> ExtractVideosAsync(string filePath)
    {
        var result = new List<SlideVideosDto>();

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
                if (!IsVideoFile(relationship.Uri.OriginalString))
                {
                    continue;
                }
                var fileName = Path.GetFileName(relationship.Uri.OriginalString);

                videos.Add(new VideoInfoDto
                {
                    FileName = fileName,
                    FilePath = relationship.Uri.OriginalString,
                    Duration = await GetVideoDurationAsync(relationship.Uri.OriginalString)
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