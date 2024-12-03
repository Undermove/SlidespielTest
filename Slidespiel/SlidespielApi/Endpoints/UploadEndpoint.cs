namespace SlidespielApi.Endpoints;

public static class UploadEndpoint
{
    public static WebApplication AddUploadEndpoint(this WebApplication app)
    {
        app.MapPost("/upload", async (IFormFile file, IFileStorageService fileStorageService) =>
        {
            if (file.Length == 0)
            {
                return Results.BadRequest(new { Message = "Файл отсутствует или пуст." });
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".pptx")
            {
                return Results.BadRequest(new { Message = "Неверный формат файла. Ожидается .pptx." });
            }

            try
            {
                // Сохранение файла
                var savedFilePath = await fileStorageService.SaveFileAsync(file);

                return Results.Ok(new
                {
                    Message = "Файл успешно загружен.",
                    FilePath = savedFilePath
                });
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        }).DisableAntiforgery();

        return app;
    }
}