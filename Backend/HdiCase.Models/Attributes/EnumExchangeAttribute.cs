
public class EnumExchangeAttribute : Attribute
{
    public readonly string exchangeName;
    public readonly string exchangeType;
    public EnumExchangeAttribute(string exchangeName, string exchangeType)
    {
        this.exchangeName = exchangeName;
        switch (exchangeType)
        {
            case "direct":
                break;
            case "fanout":
                break;
            case "topic":
                break;
            case "headers":
                break;
            default:
                throw new Exception("Exchange Type Not Found");
        }
        this.exchangeType = exchangeType;
    }
}