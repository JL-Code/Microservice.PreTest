using MassTransit;
using MassTransit.Rabbitmq;
using MassTransit.Rabbitmq.Receiver;
using System;
using System.Threading.Tasks;

namespace MassTransit.Rabbitmq.Sender
{
    class Sender
    {
        static void Main(string[] args)
        {
            Console.Title = "MassTransit Client";

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/MEVHOST"), hst =>
                {
                    hst.Username("admin");
                    hst.Password("123456");
                });
            });

            var uri = new Uri("rabbitmq://localhost/MEVHOST/CodeMe.MassTransitTest");
            var message = Console.ReadLine();

            while (message != null)
            {
                Task.Run(() => SendCommand(bus, uri, message)).Wait();
                message = Console.ReadLine();
            }

            Console.ReadKey();
        }

        private static async void SendCommand(IBusControl bus, Uri sendToUri, string message)
        {
            var endPoint = await bus.GetSendEndpoint(sendToUri);
            var command = new Client()
            {
                Id = Guid.NewGuid(),
                Name = "Me Code Client",
                Birthdate = DateTime.Now,
                Message = message
            };

            await endPoint.Send(command);

            Console.WriteLine($"You Sended : Id = {command.Id}, Name = {command.Name}, Message = {command.Message}");
        }
    }
}
