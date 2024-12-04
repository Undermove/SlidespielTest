using MediaToolkit;
using MediaToolkit.Model;

namespace SlidespielApi.Services.VideoProcessing;


public class VideoMetadataService(ILogger<VideoMetadataService> logger) : IVideoMetadataService
{
    private readonly string _ffmpegTempPath = Path.Combine(Directory.GetCurrentDirectory(), "MediaToolkitTemp/ffmpeg");

    public async Task<string> GetVideoDurationAsync(string videoPath)
    {
        if (!File.Exists(videoPath))
        {
            throw new FileNotFoundException($"Видео не найдено: {videoPath}");
        }

        var inputFile = new MediaFile { Filename = videoPath };

        return await Task.Run(() =>
        {
            try
            {
                using var engine = new Engine(_ffmpegTempPath);
                engine.GetMetadata(inputFile);

                var duration = inputFile.Metadata.Duration;
                return duration.ToString(@"hh\:mm\:ss");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception: {VideoPath}", videoPath);
                throw;
            }
        });
    }
}