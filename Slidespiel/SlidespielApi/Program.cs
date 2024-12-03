using Microsoft.OpenApi.Models;
using SlidespielApi.Endpoints;
using SlidespielApi.Endpoints.Videos;
using SlidespielApi.Infrastructure;
using SlidespielApi.Services;

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

app.AddUploadEndpoint();
app.AddVideosEndpoint();

app.Run();