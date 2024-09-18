
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class TableEntities<T> : ITableEntities
{
    [Key]
    [DataMember]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [DataMember]
    public DateTime CreatedDate { get; set; }
    [DataMember]
    public DateTime LastUpdatedDate { get; set; }
    [JsonIgnore]
    public string CollectionName { get => typeof(T).Name.ToLowerInvariant(); }
}