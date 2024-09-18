using System.Runtime.Serialization;

[DataContract]
public class AdminLoginData : TableEntities<AdminLoginData>
{
    [DataMember]
    public required string Email { get; set; }
    [DataMember]
    public required string Password { get; set; }
    [DataMember]
    public bool IsDeveloper { get; set; }
    [DataMember]
    public int[]? RoleId { get; set; }
    [DataMember]
    public bool IsActive { get; set; } = true;
}