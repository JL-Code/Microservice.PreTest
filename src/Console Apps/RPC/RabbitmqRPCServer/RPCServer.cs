using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RPC.RabbitmqRPCServer
{
    class RPCServer
    {
        const string queue_name = "rpc_queue";
        static void Main(string[] args)
        {
            //创建一个信道接收rpc请求
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue_name,
                                    durable: false,
                                    autoDelete: false,
                                    arguments: null);
                //设置未获取消息数量为1
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                //创建消费者
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: queue_name, autoAck: false, consumer: consumer);
                consumer.Received += (model, ea) =>
                {
                    //消费消息后返回响应
                    string response = null;

                    //消息参数
                    var props = ea.BasicProperties;
                    //响应参数
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var num = int.Parse(message);
                        response = Fib(num).ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" [.] " + ex.Message);
                        response = "";
                    }
                    finally
                    {
                        //发送响应消息
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                };
                Console.WriteLine(" [x] Awaiting RPC requests");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 斐波那契函数
        /// </summary>
        /// <returns></returns>
        public static long Fib(int num)
        {
            int i;
            long a = 0, b = 1, c = num;
            if (num <= 1)
                return num;
            else
            {
                for (i = 2; i <= num; i++)
                {
                    c = a + b;
                    a = b;
                    b = c;
                }
                return c;
            }
        }
    }
}
