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
}