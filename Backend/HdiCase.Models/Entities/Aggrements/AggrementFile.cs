using System.ComponentModel.DataAnnotations.Schema;

public class AggrementFile : TableEntities<AggrementFile>
{
    public required int AggrementId { get; set; }
    public required string FilePath { get; set; }
    public required string FileName { get; set; }
    public required long FileSize { get; set; }
    public required string FileText { get; set; }

    [ForeignKey("AggrementId")]
    public virtual Aggrement? Aggrement { get; set; }
}