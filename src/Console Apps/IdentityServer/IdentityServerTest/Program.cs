using IdentityModel.Client;
using System;
using System.Net.Http;

namespace IdentityServerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // discover endpoints from metadata
            var disco = DiscoveryClient.GetAsync("http://localhost:5000").GetAwaiter().GetResult();
            // request token（使用的是ClientCredentials授权类型）
            var tokenClient = new TokenClient(disco.TokenEndpoint, "payment.clientid", "payment.secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("jiangy","1").GetAwaiter().GetResult();
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var result = client.GetStringAsync("http://localhost:60811/api/values/user").GetAwaiter().GetResult();
            Console.WriteLine("Hello World! "+ result);
        }
    }
}
