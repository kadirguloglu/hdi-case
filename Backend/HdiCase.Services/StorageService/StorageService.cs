using Microsoft.AspNetCore.Http;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

public class StorageService<TCollection> : IStorageService<TCollection>
    where TCollection : class, ITableEntities
{
    private readonly TCollection _collection;
    private readonly ILogger<StorageService<TCollection>> _logger;
    public StorageService(
        ILogger<StorageService<TCollection>> logger
    )
    {
        _collection = Activator.CreateInstance<TCollection>();
        _logger = logger;
    }

    public async Task<FileBuilder?> Upload(int id, IFormFile file)
    {
        try
        {
            string directoryPath = Path.Combine($"/app/wwwroot/{_collection.CollectionName.ToString()}/{id}/{Guid.NewGuid().ToString("n")}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var fileExtension = Path.GetExtension(filePath);
            StringBuilder text = new StringBuilder();
            try
            {
                switch (fileExtension)
                {
                    case ".doc":
                    case ".docx":
                        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
                        {
                            Body? body = wordDoc?.MainDocumentPart?.Document?.Body;
                            if (body != null)
                            {
                                foreach (var para in body.Elements<Paragraph>())
                                {
                                    text.AppendLine(para.InnerText);
                                }
                            }
                        }
                        break;
                    case ".pdf":
                        using (PdfDocument document = PdfDocument.Open(filePath))
                        {
                            foreach (UglyToad.PdfPig.Content.Page page in document.GetPages())
                            {
                                string pageText = page.Text;

                                foreach (Word word in page.GetWords())
                                {
                                    text.AppendLine(word.Text);
                                }
                            }
                        }
                        break;
                    case ".txt":
                    case ".csv":
                        text.AppendLine(await File.ReadAllTextAsync(filePath));
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Read file content exception");
            }
            FileBuilder result = new FileBuilder
            {
                FileName = file.FileName,
                FilePath = filePath,
                FileSize = file.Length,
                FileText = text.ToString()
            };
            return result;
        }
        catch
        {
        }
        return null;
    }
}