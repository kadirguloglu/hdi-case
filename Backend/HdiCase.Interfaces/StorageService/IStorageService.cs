using Microsoft.AspNetCore.Http;

public interface IStorageService<TCollection>
    where TCollection : class, ITableEntities
{
    Task<FileBuilder?> Upload(int id, IFormFile file);
}

public class FileBuilder
{
    public required string FilePath { get; set; }
    public required string FileName { get; set; }
    public required long FileSize { get; set; }
    public required string FileText { get; set; }
}