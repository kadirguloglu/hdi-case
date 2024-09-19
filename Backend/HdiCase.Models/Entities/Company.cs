using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

[DataContract]
public class Company : TableEntities<Company>
{
    [DataMember]
    public required string Name { get; set; }
    [DataMember]
    public required string[] Phones { get; set; }
    [DataMember]
    public required string[] Emails { get; set; }
    [DataMember]
    public required string ApiKey { get; set; }
    [DataMember]
    public required bool ApiIsActive { get; set; }
    [DataMember]
    public required int ApiPerMinuteMaximumRequestCount { get; set; }
    [DataMember]
    public string? AggrementResultWebhookUrl { get; set; }

    public virtual List<Aggrement>? Aggrements { get; set; }
}