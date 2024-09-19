using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

[DataContract]
public class AggrementAnalysis : TableEntities<AggrementAnalysis>
{
    public int AggrementId { get; set; }
    public int UserId { get; set; }
    // hedefler
    public string? RelatedObjectives { get; set; }
    // faydalar
    public string? Benefits { get; set; }
    // efor
    public string? Efforts { get; set; }
    // maliyetler
    public string? Costs { get; set; }
    // riskler
    public string? Risks { get; set; }

    [ForeignKey("AggrementId")]
    public virtual Aggrement? Aggrement { get; set; }
    [ForeignKey("UserId")]
    public virtual AdminLoginData? AdminLoginData { get; set; }
}