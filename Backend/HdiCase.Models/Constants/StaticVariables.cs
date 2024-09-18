using RabbitMQ.Client;

public static class StaticVariables
{
    [ThreadStatic] public static bool initRabbitMQ = false;
    [ThreadStatic] public static IConnection? Connection = null;
}