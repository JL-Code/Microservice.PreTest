using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebApiClient;

namespace ClientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string _gateWayUrl;
        private readonly string _rpcUrl;
        private readonly IConfiguration _cfg;

        public ValuesController(IConfiguration cfg)
        {
            _cfg = cfg;
            _gateWayUrl = _cfg["Gateway:Url"];
            _rpcUrl = _cfg["Rpc:Url"];
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "ClientService" };
        }

        /// <summary>
        /// 调用Payment服务返回指定账号的支付记录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("{account}")]
        public async Task<ActionResult<string>> GetAsync(string account)
        {
            try
            {
                using (var client = HttpApiClient.Create<IPaymentApi>(_gateWayUrl))
                {
                    var historyList = await client.GetPaymentHistoryAsync(account);
                    return Ok(historyList);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// 调用Payment服务返回指定账号的支付记录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("rpc/{account}")]
        public ActionResult<string> Rpc(string account)
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new Payment.Payment.PaymentClient(channel);

            string user = "you";
            //注意 此处若使用异步方法则会导致channel关闭耗时增加10s左右，原因未知！！！！
            var reply = client.GetPaymentHistory(new Payment.RPCRequest { Account = user });
            var message = reply.Message;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            channel.ShutdownAsync().Wait();
            watch.Stop();
            var total = watch.Elapsed.TotalMilliseconds;
            return Ok(message + "耗时：" + total);

        }

        [Authorize]
        [HttpGet("user/info")]
        public IActionResult GetUserInfo()
        {
            return Ok(new
            {
                UserName = "jiangy",
                Age = 26
            });
        }
    }
}
