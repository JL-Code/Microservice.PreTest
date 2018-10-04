using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient;

namespace ClientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string _gateWayUrl;
        private readonly IConfiguration _cfg;

        public ValuesController(IConfiguration cfg)
        {
            _cfg = cfg;
            _gateWayUrl = _cfg["Gateway:Url"];
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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

    }
}
