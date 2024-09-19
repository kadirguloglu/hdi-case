using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

[DataContract]
public class Aggrement : TableEntities<Aggrement>
{
    [DataMember]
    public DateTime StartDate { get; set; }
    [DataMember]
    public DateTime EndDate { get; set; }
    [DataMember]
    public int RiskRate { get; set; }
    [DataMember]
    public decimal RiskAmount { get; set; }
    [DataMember]
    public int CompanyId { get; set; }
    [DataMember]
    public Enum_AggrementStatus Status { get; set; }
    [DataMember]
    public string? RejectDescription { get; set; }

    [ForeignKey("CompanyId")]
    public virtual Company? Company { get; set; }
    public virtual List<AggrementAnalysis>? CompanyAggrementAnalysis { get; set; }
    public virtual List<AggrementFile>? AggrementFiles { get; set; }
    public virtual List<AggrementContact>? AggrementContacts { get; set; }
}