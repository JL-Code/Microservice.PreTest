using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Payment.Service.Controllers
{
    /// <summary>
    /// 支付
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        /// <summary>
        /// 获取指定账号的交易记录
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [HttpGet("history/{account}")]
        public IActionResult GetPaymentHistory(string account) {
            IList<string> historyList = new List<string>
            {
                "2018-06-10,10000RMB,Chengdu",
                "2018-06-11,11000RMB,Chengdu",
                "2018-06-12,12000RMB,Beijing",
                "2018-06-13,10030RMB,Chengdu",
                "2018-06-20,10400RMB,HongKong"
            };
            return Ok(historyList);
        }
    }
}