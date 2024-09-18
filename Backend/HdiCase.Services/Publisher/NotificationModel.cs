using System.Runtime.Serialization;

[DataContract]
public class NotificationModel : INotificationModel
{
    [DataMember]
    public Enum_NotificationType NotificationType { get; set; }
    // bildirimin gidecegi kisi
    [DataMember]
    public string? UserId { get; set; }
    [DataMember]
    public int Chuck { get; set; }
}