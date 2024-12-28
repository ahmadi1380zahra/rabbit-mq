using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq;

public static class Consumer
{
    public static async Task ConsumeDirect()
    {
        var factory= new ConnectionFactory() { HostName = "localhost" };
        using (var connection =await factory.CreateConnectionAsync())
        {
            using (var channel = await connection.CreateChannelAsync())
            {
                var queueName = "meet-queue"; 
                await channel.QueueDeclareAsync(queue: queueName,
                    durable: false, exclusive: false,
                    autoDelete: false,
                    arguments: null);
                await  channel.QueueBindAsync(queue: queueName, exchange: "meet-friendly", routingKey: "meeting");
                
                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                };

                await channel.BasicConsumeAsync(queue: queueName,
                    autoAck: false,  
                    consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press [CTRL+C]");
                Console.ReadLine();
            }
            
        }
    }
    public static async Task ConsumeTopic()
    {
        var factory= new ConnectionFactory() { HostName = "localhost" };
        using (var connection =await factory.CreateConnectionAsync())
        {
            using (var channel = await connection.CreateChannelAsync())
            {
                var queueName = "meet-queue-topic"; 
                await channel.QueueDeclareAsync(queue: queueName,
                    durable: false, exclusive: false,
                    autoDelete: false,
                    arguments: null);
                await  channel.QueueBindAsync(queue: queueName, exchange: "meet-friendly-topic-exhchange", routingKey: "meeting.*.location");
                
                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                };

                await channel.BasicConsumeAsync(queue: queueName,
                    autoAck: false,  
                    consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press [CTRL+C]");
                Console.ReadLine();
            }
            
        }
    }
    public static async Task ConsumeTopic2()
    {
        var factory= new ConnectionFactory() { HostName = "localhost" };
        using (var connection =await factory.CreateConnectionAsync())
        {
            using (var channel = await connection.CreateChannelAsync())
            {
                var queueName = "meet-queue-topic2"; 
                await channel.QueueDeclareAsync(queue: queueName,
                    durable: false, exclusive: false,
                    autoDelete: false,
                    arguments: null);
                await  channel.QueueBindAsync(queue: queueName, exchange: "meet-friendly-topic-exhchange", routingKey: "meeting.#");
                
                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                };

                await channel.BasicConsumeAsync(queue: queueName,
                    autoAck: false,  
                    consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press [CTRL+C]");
                Console.ReadLine();
            }
            
        }
    }
}