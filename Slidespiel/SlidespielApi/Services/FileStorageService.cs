namespace SlidespielApi.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _storageDirectory;
    private readonly string _videoStoragePath;

    public FileStorageService()
    {
        _storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        
        if (!Directory.Exists(_storageDirectory))
        {
            Directory.CreateDirectory(_storageDirectory);
        }
        
        _videoStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "DownloadedVideos");
        
        if (!Directory.Exists(_videoStoragePath))
        {
            Directory.CreateDirectory(_videoStoragePath);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл отсутствует или пуст.", nameof(file));
        
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(_storageDirectory, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(stream);

        return filePath;
    }
    
    public Task<bool> FileExistsAsync(string filePath)
    {
        return Task.FromResult(File.Exists(filePath));
    }
    
    public Task<Stream> GetFileStreamAsync(string filePath)
    {
        var fullPath = Path.Combine(_videoStoragePath, filePath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        // Возвращаем поток для чтения файла
        return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read));
    }
}