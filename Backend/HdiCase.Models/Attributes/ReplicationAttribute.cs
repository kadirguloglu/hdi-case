[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ReplicationAttribute : Attribute
{
    public required string DatabaseName { get; set; }
    public required string ConnectionStringName { get; set; }
}