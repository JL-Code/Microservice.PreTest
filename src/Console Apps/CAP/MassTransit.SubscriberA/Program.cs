using MassTransit.Messages;
using System;
using System.Threading.Tasks;

namespace MassTransit.SubscriberA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SubscriberA：我只接收TestBaseMessage类型的消息，其他的我不要";

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/MEVHOST"), hst =>
                {
                    hst.Username("admin");
                    hst.Password("123456");
                });

                cfg.ReceiveEndpoint(host, "Queue.MassTransit.CA", e =>
                {
                    e.Consumer<ConsumerA>();
                });
            });

            bus.Start();
            Console.ReadKey(); // press Enter to Stop
            bus.Stop();
        }
    }

    public class ConsumerA : IConsumer<TestBaseMessage>
    {
        public async Task Consume(ConsumeContext<TestBaseMessage> context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync($"SubscriberA => ConsumerA received message : {context.Message.Name}, {context.Message.Time}, {context.Message.Message}, Type:{context.Message.GetType()}");
            Console.ResetColor();
        }
    }
}
