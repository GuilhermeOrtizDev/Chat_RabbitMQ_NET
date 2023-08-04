using Chat.Web.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Chat.Web
{
    public class Rabbitmq
    {
        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Rabbitmq(
            IOptions<RabbitMqConfiguration> option)
        {
            _configuration = option.Value;
            var factory = new ConnectionFactory
            {
                HostName = _configuration.Host
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: _configuration.Queue,
                exclusive: false,
                durable: false,
                autoDelete: false,
                arguments: null);
        }

        public void Send(MessageInputModel message)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);
            var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

            _channel.BasicPublish(
                exchange: "",
                routingKey: _configuration.Queue,
                basicProperties: null,
                body: bytesMessage);
        }

        public MessageInputModel? Received()
        {
            var consumer = new EventingBasicConsumer(_channel);
            var result = _channel.BasicGet(queue: _configuration.Queue, autoAck: true);

            if (result == null) return null;

            var contentArray = result.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            return JsonConvert.DeserializeObject<MessageInputModel>(contentString);
        }
    }
}
