using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Rebbitmq.Consumer
{
    public class RabbitmqSubscriberClient
    {
        const string EXCHANGE_NAME = "";
        const string QUEUE_NAME = "hello";
        const string ROUTING_KEY = "hello";

        public void Receive()
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            //创建信道
            using (IModel channel = connection.CreateModel())
            {
                //声明队列
                channel.QueueDeclare(queue: QUEUE_NAME,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                //事件消费者
                var consumer = new EventingBasicConsumer(channel);
                consumer.Registered += Consumer_Registered;
                consumer.Received += Consumer_Received;

                //告诉rabbitmq消息收到了
                channel.BasicConsume(queue: QUEUE_NAME,
                           autoAck: true,
                           consumer: consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("[x] 接收到的消息：{0}", message);
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            Console.WriteLine("消费者注册成功");
        }
    }

}
