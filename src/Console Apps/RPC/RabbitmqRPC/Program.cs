using System;
using System.Diagnostics;

namespace RPC.RabbitmqRPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RPCClient();
            Console.WriteLine("请输入num");
            var num = Console.ReadLine();
            while (num != "exit")
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = rpcClient.Call(num);
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                Console.WriteLine("第{0}项的数值为：{1}，计算总耗时：{2}毫秒", num, response, timeSpan.TotalMilliseconds);
                Console.WriteLine("请输入num");
                num = Console.ReadLine();
            }
            rpcClient.Close();
            Console.WriteLine("connection Close");
        }
    }
}
