using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentAPI.RabbitMQConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IProccessPayment _proccessPayment;
        public RabbitMQPaymentConsumer(IProccessPayment proccessPayment)
        {
            _proccessPayment = proccessPayment;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "orderpaymentproccessqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PaymentMessage message = JsonSerializer.Deserialize<PaymentMessage>(content);
                ProccessPayment(message).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("orderpaymentproccessqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProccessPayment(PaymentMessage message)
        {
            try
            {
                //_rabbitMQMessageSender.SendMessage(payment, "orderpaymentproccessqueue");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
