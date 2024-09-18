
using System.Runtime.Serialization;

[DataContract]
public class Role : TableEntities<Role>
{
    [DataMember]
    public required string Name { get; set; }
    [DataMember]
    public string? Description { get; set; }
    [DataMember]
    public int[]? PermissionKeys { get; set; }
}