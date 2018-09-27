using System;

namespace Rebbitmq.Consumer
{
    class Subscriber
    {
        static void Main(string[] args)
        {
            var subscriber = new RabbitmqSubscriberClient();
            Console.WriteLine("有人发来消息:");
            subscriber.Receive();
            Console.ReadLine();
        }
    }
}
