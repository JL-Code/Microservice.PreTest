using System;

namespace MassTransit.Rabbitmq.Receiver
{
    class Receiver
    {
        static void Main(string[] args)
        {
            Console.Title = "MassTransit Server";

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/MEVHOST"), hst =>
                {
                    hst.Username("admin");
                    hst.Password("123456");
                });

                cfg.ReceiveEndpoint(host, "CodeMe.MassTransitTest", e =>
                {
                    e.Consumer<TestConsumerClient>();
                    e.Consumer<TestConsumerAgent>();
                });
            });

            bus.Start();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            bus.Stop();
        }
    }
}
