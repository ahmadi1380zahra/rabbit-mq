using System.Text;
using RabbitMQ.Client;

namespace RabbitMq;

public static class Producer
{
    public static async Task ProduceDirect()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = await factory.CreateConnectionAsync())
        {
            using (var channel = await connection.CreateChannelAsync())
            {
                string exchangeName = "meet-friendly";
                await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct);
                string message = "Hello World the mision is to meet my friend!";
                var body = Encoding.UTF8.GetBytes(message);
                var basicProperties = new BasicProperties();
                await channel.BasicPublishAsync(
                    exchange: exchangeName,
                    routingKey: "meeting",
                    mandatory: false,
                    basicProperties: basicProperties,
                    body: body,
                    cancellationToken: CancellationToken.None
                );
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
    public static async Task ProduceTopic()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = await factory.CreateConnectionAsync())
        {
            using (var channel = await connection.CreateChannelAsync())
            {
                string exchangeName = "meet-friendly-topic-exhchange";
                await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic);
                string message = "Hello World the mision is to meet my friend wih topic exhchange!";
                var body = Encoding.UTF8.GetBytes(message);
                string routingKey = "meeting.friends.location";
                var basicProperties = new BasicProperties();
                await channel.BasicPublishAsync(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: basicProperties,
                    body: body,
                    cancellationToken: CancellationToken.None
                );
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}