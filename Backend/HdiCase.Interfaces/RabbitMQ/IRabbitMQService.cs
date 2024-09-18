using RabbitMQ.Client;
public interface IRabbitMQService
{
    IConnection? Connection { get; }
    bool IsConnected { get; }
    IModel? GetModel(IConnection connection);
    void InitRabbitMQ();
}