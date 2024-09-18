using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

public class RabbitMQService : IRabbitMQService
{
    private readonly ILogger<RabbitMQService> _logger;
    public IConnection? Connection
    {
        get
        {
            if (StaticVariables.Connection != null && StaticVariables.Connection.IsOpen)
            {
                return StaticVariables.Connection;
            }
            StaticVariables.Connection = GetConnection();
            if (!StaticVariables.initRabbitMQ)
            {
                StaticVariables.initRabbitMQ = true;
                InitRabbitMQ();
            }
            return StaticVariables.Connection;
        }
    }

    int retryCount = 0;
    public RabbitMQService(
        ILogger<RabbitMQService> logger)
    {
        _logger = logger;
    }

    public IConnection? GetConnection()
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = EnvironmentSettings.RabbitMqHost,
                UserName = EnvironmentSettings.RabbitMqUsername,
                Password = EnvironmentSettings.RabbitMqPassword,
                Port = Convert.ToInt32(EnvironmentSettings.RabbitMqPort)
            };

            // Otomatik bağlantı kurtarmayı etkinleştirmek için,
            factory.AutomaticRecoveryEnabled = true;
            // Her 10 sn de bir tekrar bağlantı toparlanmaya çalışır 
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            // sunucudan bağlantısı kesildikten sonra kuyruktaki mesaj tüketimini sürdürmez 
            // (TopologyRecoveryEnabled = false   olarak tanımlandığı için)
            factory.TopologyRecoveryEnabled = false;

            var connectionResult = factory.CreateConnection();
            return connectionResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ Connection Failed");
            // loglama işlemi yapabiliriz
            Thread.Sleep(500);
            // farklı business ta yapılabilir, ancak biz tekrar bağlantı (connection) kurmayı deneyeceğiz
            retryCount++;
            if (retryCount > 2)
            {
                _logger.LogError("RabbitMQ Connection Failed");
                return null;
            }
            return GetConnection();
        }
    }

    public bool IsConnected
    {
        get
        {
            if (Connection is null || !Connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ IsConnected State False");
                return false;
            }
            return true;
        }
    }

    public IModel? GetModel(IConnection connection)
    {
        if (!IsConnected)
        {
            Console.WriteLine("RabbitMQ GetModel State false");
            return null;
        }
        return connection.CreateModel();
    }

    public void InitRabbitMQ()
    {
        try
        {
            if (!IsConnected) return;
            if (Connection is null || !Connection.IsOpen)
            {
                _logger.LogError("RabbitMQ Connection Failed");
                return;
            }
            using var channel = GetModel(Connection);
            if (channel is null)
            {
                _logger.LogError("RabbitMQ Channel Failed");
                return;
            }
            foreach (var item in EnumExtensions.GetEnumAttributes<EnumExchangeAttribute, Enum_RabbitMQExchanges>())
            {
                channel.ExchangeDeclare(item.exchangeName, type: item.exchangeType, true, false);
            }
            foreach (var item in EnumExtensions.GetEnumAttributes<EnumQueueAttribute, Enum_RouteKeyOrQueueName>())
            {
                channel.QueueDeclare(item.queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(item.queue, item.exchange, item.routingKey, item.arguments);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("RabbitMQ Init Exception : ", ex.Message);
        }
    }
}