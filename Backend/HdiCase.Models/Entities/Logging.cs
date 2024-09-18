using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

[DataContract]
public class Logging : TableEntities<Logging>
{
    [DataMember]
    public int? UserId { get; set; }
    [DataMember]
    public string? TableName { get; set; }
    [DataMember]
    public int? TableId { get; set; }
    [DataMember]
    public Enum_OperationType? OperationType { get; set; }
    [DataMember]
    public string? IpAddress { get; set; }
    [DataMember]
    public string? OldData { get; set; }
    [DataMember]
    public string? NewData { get; set; }

    [ForeignKey("UserId")]
    public virtual AdminLoginData? AdminLoginData { get; set; }
}