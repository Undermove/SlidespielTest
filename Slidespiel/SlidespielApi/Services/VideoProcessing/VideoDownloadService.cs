using System.Net.Http.Headers;

namespace SlidespielApi.Services.VideoProcessing;

public class VideoDownloadService(HttpClient httpClient) : IVideoDownloadService
{
    public async Task<string> DownloadVideoAsync(string videoUrl, string destinationFolder)
    {
        if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out var uri))
        {
            throw new ArgumentException("Неверный URL видео.", nameof(videoUrl));
        }

        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }
        
        using var response = await httpClient.GetAsync(videoUrl, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        
        var fileName = GetFileNameFromResponse(response) ?? Path.GetFileName(uri.LocalPath);
        if (string.IsNullOrEmpty(fileName))
        {
            throw new InvalidOperationException("Не удалось определить имя файла.");
        }

        var filePath = Path.Combine(destinationFolder, fileName);
        
        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream);

        return filePath;
    }

    private string? GetFileNameFromResponse(HttpResponseMessage response)
    {
        string? fileName;
        if (response.Content.Headers.ContentDisposition != null)
        {
            fileName = response.Content.Headers.ContentDisposition.FileNameStar ?? response.Content.Headers.ContentDisposition.FileName;
            return fileName?.Trim('"'); 
        }

        if (!response.Headers.Contains("Content-Disposition"))
        {
            return null;
        }
        
        var contentDisposition = response.Headers.GetValues("Content-Disposition").FirstOrDefault();
        if (!ContentDispositionHeaderValue.TryParse(contentDisposition, out var parsedDisposition))
        {
            return null;
        }
        fileName = parsedDisposition.FileNameStar ?? parsedDisposition.FileName;
        return fileName?.Trim('"');
    }
}