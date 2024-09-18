using System.ComponentModel;
using RabbitMQ.Client;

public enum Enum_RabbitMQExchanges
{
    [EnumExchangeAttribute("notificationExchange", ExchangeType.Headers)]
    NotificationExchange = 1
}