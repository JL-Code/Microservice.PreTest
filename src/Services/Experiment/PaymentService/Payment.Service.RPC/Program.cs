using Grpc.Core;
using Payment.Application;
using Payment.Application.Implement;
using System;

namespace Payment.Service.RPC
{
    class Program
    {
        const int Port = 50051;
        const string Host = "localhost";

        public static void Main(string[] args)
        {
            IPaymentService service = new PaymentService();
            Server server = new Server
            {
                Services = { Payment.BindService(new PaymentImpl(service)) },
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
