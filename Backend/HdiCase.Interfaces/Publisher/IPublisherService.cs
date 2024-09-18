public interface IPublisherService<T>
    where T : class, INotificationModel
{
    Task Enqueue(T model,
        Enum_RabbitMQExchanges exchange,
        Dictionary<Enum_RabbitMQHeaderKeys, Enum_RabbitMQHeaderValues> headers);
    Task Enqueue(T model,
        Enum_RabbitMQExchanges exchange,
        Enum_RouteKeyOrQueueName routeKeyOrQueueName);
    Task Enqueue(T model,
        Enum_RabbitMQExchanges exchange,
        Enum_RouteKeyOrQueueName? routeKeyOrQueueName,
        Dictionary<Enum_RabbitMQHeaderKeys, Enum_RabbitMQHeaderValues>? headers);
}