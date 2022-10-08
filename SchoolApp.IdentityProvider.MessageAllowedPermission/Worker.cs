using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SchoolApp.IdentityProvider.MessageAllowedPermission.Dtos;
using SchoolApp.IdentityProvider.MessageAllowedPermission.Settings;

namespace SchoolApp.IdentityProvider.MessageAllowedPermission;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;

    public Worker(ILogger<Worker> logger, IOptions<MessageAllowedPermissionQueueSettings> options)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(options.Value.ConnectionString)
        };
        _connection = factory.CreateConnection();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Waiting messages...");

        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: "message.allowed.permission",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;
            channel.BasicConsume(queue: "message.allowed.permission",
                autoAck: true,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Active worker: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    private void ConsumerReceived(object sender, BasicDeliverEventArgs e)
    {
        var stringMessage = Encoding.UTF8.GetString(e.Body.ToArray());
        var message = System.Text.Json.JsonSerializer.Deserialize<MessageAllowedPermissionDto>(stringMessage);
    }

    public override void Dispose()
    {
        _connection.Dispose();
        base.Dispose();
    }
}
