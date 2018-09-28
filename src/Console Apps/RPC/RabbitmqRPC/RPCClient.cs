using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace RPC.RabbitmqRPCClient
{
    public class RPCClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly IBasicProperties props;

        public RPCClient()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            //声明一个匿名队列
            replyQueueName = channel.QueueDeclare().QueueName;
            props = channel.CreateBasicProperties();

            //关联ID 用于确认请求/响应是一对
            props.CorrelationId = Guid.NewGuid().ToString();
            props.ReplyTo = replyQueueName; //用于告诉RPCServer响应地址

            //创建一个事件消费者
            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                //通过关联ID判断响应是否为之前发送的请求的响应
                if (ea.BasicProperties.CorrelationId == props.CorrelationId)
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body);
                    respQueue.Add(response);
                }
            };

        }

        /// <summary>
        /// 负责实现Rabbitmq通信调用远程方法
        /// </summary>
        /// <param name="num"></param>
        public string Call(string num)
        {
            //发送消息
            var message = Encoding.UTF8.GetBytes(num);
            //发送RPC调用
            channel.BasicPublish(exchange: "",
                                routingKey: "rpc_queue",
                                basicProperties: props,
                                body: message);
            //启动消费者接收响应消息
            channel.BasicConsume(queue: replyQueueName,
                                autoAck: true,//自动确认
                                consumer: consumer);

            return respQueue.Take();
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
