using Journal.Infrastructure.MessageBus.Queues;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Journal.Infrastructure.MessageBus
{
    public class Publisher<TEntity> : IPublisher<TEntity>
    {
        private readonly IConfiguration _configuration;

        public Publisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(TEntity message, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration.GetSection("RabbitMQ").Value)
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: ExchangesName.DeadLetterExchange,
                                    type: ExchangeType.Fanout,
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(QueuesName.JournalDeadLetter, true, false, false, null);
            channel.QueueBind(QueuesName.JournalDeadLetter, ExchangesName.DeadLetterExchange, "");

            var arguments = new Dictionary<string, object>()
            {
                {"x-dead-letter-exchange", ExchangesName.DeadLetterExchange }
            };

            channel.QueueDeclare(QueuesName.JournalQueue, true, false, false, arguments);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            await Task.Run(() =>
            {
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            });
        }
    }
}