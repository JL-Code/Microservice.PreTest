using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    public class ButterflyController : Controller
    {
        HttpClient _httpClient;
        string _gatewayUri;
        public ButterflyController(HttpClient httpClient, IConfiguration cfg)
        {
            _gatewayUri = $"http://{cfg["Gateway:IPAddress"]}:{cfg["Gateway:Port"]}";
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            // discover endpoints from metadata
            var disco = DiscoveryClient.GetAsync("http://localhost:5000").GetAwaiter().GetResult();
            // request token（使用的是ClientCredentials授权类型）
            var tokenClient = new TokenClient(disco.TokenEndpoint, "payment.clientid", "payment.secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("jiangy", "1").GetAwaiter().GetResult();
            if (tokenResponse.IsError)
            {
                ViewData["Message"] = $"Your request data result : {tokenResponse.Error}";
                return View();
            }

            _httpClient.SetBearerToken(tokenResponse.AccessToken);
            var requestResult = _httpClient.GetStringAsync($"{_gatewayUri}/payment/values").GetAwaiter().GetResult();

            ViewData["Message"] = $"Your request data result : {requestResult}";
            return View();
        }
    }
}