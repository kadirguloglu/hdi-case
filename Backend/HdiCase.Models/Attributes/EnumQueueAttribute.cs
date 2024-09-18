

public class EnumQueueAttribute : Attribute
{
    public string queue { get; set; }
    public string exchange { get; set; }
    public string routingKey { get; set; }
    public Dictionary<string, object>? arguments { get; set; }
    public EnumQueueAttribute(
        string queue,
        Enum_RabbitMQExchanges exchange,
        string routingKey,
        string? xMatch1 = null,
        string? type1 = null,
        string? xMatch2 = null,
        string? type2 = null)
    {
        this.queue = queue;
        this.exchange = EnumExtensions.GetEnumAttribute<EnumExchangeAttribute>(exchange)?.exchangeName ?? "";
        this.routingKey = routingKey;
        if (!string.IsNullOrEmpty(xMatch1) && !string.IsNullOrEmpty(type1))
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add(xMatch1, type1);
            if (!string.IsNullOrEmpty(xMatch2) && !string.IsNullOrEmpty(type2))
            {
                args.Add(xMatch2, type2);
            }
            arguments = args;
        }
    }

}