using RabbitMQ.Client;
using System.Text;

namespace Rabbitmq.Publisher
{
    public class RabbitmqPublisherClient
    {
        const string EXCHANGE_NAME = "";
        const string QUEUE_NAME = "hello";
        const string ROUTING_KEY = "hello";

        public void Send(string message)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            {
                //创建信道
                using (IModel channel = connection.CreateModel())
                {
                    //声明交换模式
                    //channel.ExchangeDeclare(EXCHANGE_NAME, EXCHANGE_MODE.DIRECT, true, false, null);
                    //声明队列
                    channel.QueueDeclare(queue: QUEUE_NAME, durable: false, autoDelete: false, exclusive: false, arguments: null);
                    ////队列绑定交换器
                    //channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, ROUTING_KEY, null);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: EXCHANGE_NAME,
                        routingKey: ROUTING_KEY,
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }
}
