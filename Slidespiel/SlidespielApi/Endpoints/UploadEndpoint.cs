namespace SlidespielApi.Endpoints;

public static class UploadEndpoint
{
    public static WebApplication AddUploadEndpoint(this WebApplication app)
    {
        app.MapPost("/upload", async () =>
        { });

        return app;
    }
}