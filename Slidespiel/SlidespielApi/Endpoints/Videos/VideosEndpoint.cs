using SlidespielApi.Services;

namespace SlidespielApi.Endpoints.Videos;

public static class VideosEndpoint
{
    public static WebApplication AddVideosEndpoint(this WebApplication app)
    {
        // Эндпоинт: Получить список всех видео, сгруппированных по слайдам
        app.MapGet("/videos/list", async (string filePath, IPowerPointParser parser, IFileStorageService storage) =>
            {
                if (string.IsNullOrEmpty(filePath) || !await storage.FileExistsAsync(filePath))
                {
                    return Results.NotFound(new { Message = "Презентация не найдена." });
                }

                try
                {
                    var videoData = await parser.ExtractVideosAsync(filePath);
                    return Results.Ok(videoData);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetVideoList")
            .WithTags("Videos API")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .DisableAntiforgery();

        return app;
    }
    
    public static WebApplication AddDownloadVideoEndpoint(this WebApplication app)
    {
        app.MapGet("/download-video", async (string filePath, IFileStorageService fileStorageService) =>
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return Results.BadRequest(new { Message = "Путь к файлу не указан." });
                }

                if (!await fileStorageService.FileExistsAsync(filePath))
                {
                    return Results.NotFound(new { Message = "Файл не найден." });
                }

                try
                {
                    var videoStream = await fileStorageService.GetFileStreamAsync(filePath);
                    var fileName = Path.GetFileName(filePath);

                    return Results.File(videoStream, "application/octet-stream", fileName);
                }
                catch (Exception ex)
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("DownloadVideo")
            .WithTags("Video API")
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .DisableAntiforgery();

        return app;
    }
}