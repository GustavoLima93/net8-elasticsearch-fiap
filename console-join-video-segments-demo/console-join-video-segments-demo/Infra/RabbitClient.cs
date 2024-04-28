using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace console_join_video_segments_demo.Infra;

public interface IRabbitClient
{
     event Action<string> MessageReceived;
     void ConsumeMessage(string queue);
}

public class RabbitClient : IRabbitClient
{
    private readonly IModel _channel;
    public event Action<string> MessageReceived;

    public RabbitClient(IRabbitSettings settings)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(settings.ConnectionString)
        };
        var connection = factory.CreateConnection();
        this._channel = connection.CreateModel();
    }

    public void ConsumeMessage(string queue)
    {
        
        this._channel.QueueDeclare(queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        var consumer = new EventingBasicConsumer(this._channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            OnMessageReceived(message);
        };

        this._channel.BasicConsume(queue: queue,
            autoAck: true,
            consumer: consumer);
    }
    
    protected virtual void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(message);
    }
}