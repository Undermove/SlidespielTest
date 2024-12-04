using Microsoft.OpenApi.Models;
using SlidespielApi.Endpoints;
using SlidespielApi.Endpoints.Videos;
using SlidespielApi.Infrastructure;
using SlidespielApi.Services;
using SlidespielApi.Services.VideoProcessing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "PowerPoint Video API",
        Description = "API для работы с презентациями PowerPoint и встроенными видеофайлами"
    });
    options.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddSingleton<IVideoMetadataService, VideoMetadataService>();
builder.Services.AddHttpClient<IVideoDownloadService, VideoDownloadService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddSingleton<IFileStorageService, FileStorageService>();
builder.Services.AddSingleton<IPowerPointParser, PowerPointParser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PowerPoint Video API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionsMiddleware>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
AppDomain.CurrentDomain.UnhandledException += (_, args) =>
{
    if (args.ExceptionObject is Exception exception)
    {
        logger.LogError(exception, "Unhandled exception occurred");
    }
    else
    {
        logger.LogError("Unhandled non-exception error occurred: {ExceptionObject}", args.ExceptionObject);
    }
};

TaskScheduler.UnobservedTaskException += (_, args) =>
{
    logger.LogError(args.Exception, "Unobserved task exception occurred");
    args.SetObserved();
};

app.AddUploadPresentationEndpoint();
app.AddVideosEndpoint();
app.AddDownloadVideoEndpoint();

app.Run();