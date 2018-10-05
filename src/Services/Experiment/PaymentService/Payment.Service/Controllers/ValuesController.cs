using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Payment.Service.Controllers
{
    /// <summary>
    /// 默认
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "payment.service1", "payment.service2" };
        }
    }
}