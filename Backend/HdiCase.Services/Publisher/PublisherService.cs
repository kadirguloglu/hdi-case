
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class PublisherService<T> : IPublisherService<T>
    where T : class, INotificationModel
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<PublisherService<T>> _logger;
    public PublisherService(
        IRabbitMQService rabbitMQService,
        ILogger<PublisherService<T>> logger
    )
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    public async Task Enqueue(T model, Enum_RabbitMQExchanges exchange, Enum_RouteKeyOrQueueName routeKey)
    {
        await Enqueue(model, exchange, routeKey, null);
    }

    public async Task Enqueue(T model, Enum_RabbitMQExchanges exchange, Dictionary<Enum_RabbitMQHeaderKeys, Enum_RabbitMQHeaderValues> headers)
    {
        await Enqueue(model, exchange, default, headers);
    }

    public async Task Enqueue(T model, Enum_RabbitMQExchanges exchange, Enum_RouteKeyOrQueueName? routeKey, Dictionary<Enum_RabbitMQHeaderKeys, Enum_RabbitMQHeaderValues>? headers)
    {
        await Task.Run(() =>
        {
            try
            {
                if (!_rabbitMQService.IsConnected)
                    return;
                using var connection = _rabbitMQService.Connection;
                if (connection is null || !connection.IsOpen)
                    return;
                using var channel = _rabbitMQService.GetModel(connection);
                if (channel is null)
                    return;
                var properties = channel.CreateBasicProperties();
                if (headers != null)
                {
                    var DictionaryHeaders = new Dictionary<string, object>();
                    foreach (var item in headers)
                    {
                        DictionaryHeaders.Add(item.Key.ToString(), item.Value.ToString());
                    }
                    properties.Headers = DictionaryHeaders;
                }
                var modelString = JsonSerializer.Serialize(model);
                var body = Encoding.UTF8.GetBytes(modelString);
                _logger.LogInformation("Add New Queue = " + modelString);
                channel.BasicPublish(exchange: EnumExtensions.GetEnumAttribute<EnumExchangeAttribute>(exchange)?.exchangeName ?? "",
                                     routingKey: routeKey != null ? EnumExtensions.GetEnumAttribute<EnumQueueAttribute>(routeKey)?.queue ?? "" : "",
                                     mandatory: false,
                                     basicProperties: properties,
                                     body: body);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Enqueue Exception : ", ex.Message);
                _logger.LogError(ex, "PublisherService Add Queue");
            }
        });
    }
}