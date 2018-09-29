using System;

namespace Rebbitmq.Consumer
{
    class Subscriber
    {
        static void Main(string[] args)
        {
            var subscriber = new RabbitmqSubscriberClient();
            //var subscriber1 = new RabbitmqSubscriberClient();
            //var subscriber2 = new RabbitmqSubscriberClient();
            Console.WriteLine("有人发来消息:");
            subscriber.Receive();
            //subscriber1.Receive();
            //subscriber2.Receive();
            Console.ReadLine();
        }
    }
}
