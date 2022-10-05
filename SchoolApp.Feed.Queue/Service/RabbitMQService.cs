using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SchoolApp.Feed.Queue.Settings;

namespace SchoolApp.Feed.Queue.Service;

public abstract class RabbitMQService<TEntity> : IDisposable
{
    protected string QueueName { get; set; }
    private readonly IConnection _connection;

    public RabbitMQService(IOptions<RabbitMQSettings> options, string queueName)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(options.Value.ConnectionString)
        };

        QueueName = queueName;
        _connection = factory.CreateConnection();
    }

    public void Send(TEntity message)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: QueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.BasicPublish(exchange: "",
                     routingKey: QueueName,
                     basicProperties: null,
                     body: Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message)));
        }
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
