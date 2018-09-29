using System;

namespace Rabbitmq.Publisher
{
    class Publisher
    {
        static void Main(string[] args)
        {
            string input = "";
            var publisher = new RabbitmqPublisherClient();
            Console.WriteLine("请输入字符串,输入exit退出");
            while (input?.ToLower() != "exit")
            {
                input = Console.ReadLine();
                publisher.Send(input);
                publisher.Send(input);
                publisher.Send(input);
                Console.WriteLine("你输入字符串是：" + input);
            }
        }
    }
}
