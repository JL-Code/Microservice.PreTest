using MassTransit.Messages;
using System;
using System.Threading.Tasks;

namespace MassTransit.SubscriberB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "MassTransit SubscriberB 我只接收TestCustomMessage类型的消息，其他的我不要";

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/MEVHOST"), hst =>
                {
                    hst.Username("admin");
                    hst.Password("123456");
                });

                cfg.ReceiveEndpoint(host, "Queue.MassTransit.CB", e =>
                {
                    e.Consumer<ConsumerA>();
                });
            });

            bus.Start();
            Console.ReadKey(); // press Enter to Stop
            bus.Stop();
        }

    }

    public class ConsumerA : IConsumer<TestCustomMessage>
    {
        public async Task Consume(ConsumeContext<TestCustomMessage> context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync($"SubscriberB => ConsumerA received message : {context.Message.Name}, {context.Message.Time}, {context.Message.Message}, Age: {context.Message.Age}, Type:{context.Message.GetType()}");
            Console.ResetColor();
        }
    }
}
