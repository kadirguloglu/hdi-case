using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

[DataContract]
public class AggrementContact : TableEntities<AggrementContact>
{
    [DataMember]
    public required string NameSurname { get; set; }
    [DataMember]
    public string? Phone { get; set; }
    [DataMember]
    public string? Email { get; set; }
    [DataMember]
    public int AggrementId { get; set; }

    [ForeignKey("AggrementId")]
    public virtual Aggrement? Aggrement { get; set; }
}