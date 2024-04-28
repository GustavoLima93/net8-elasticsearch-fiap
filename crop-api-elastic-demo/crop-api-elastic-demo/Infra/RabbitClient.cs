using System.Text;
using RabbitMQ.Client;

namespace crop_api_elastic_demo.Infra;

public interface IRabbitClient
{ 
    void SendMessage(string queue, string message);
}

public class RabbitClient: IRabbitClient
{
    private readonly IModel _channel;

    public RabbitClient(IRabbitSettings settings)
    {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(settings.ConnectionString);
            var connection = factory.CreateConnection();
            this._channel  = connection.CreateModel();
    }

    public void SendMessage(string queue, string message)
    {
        this._channel.QueueDeclare(queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        this._channel.BasicPublish(exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(message));
    }
}