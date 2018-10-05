using Microsoft.AspNetCore.Mvc;
using System;
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
            var info = $"payment.service: {DateTime.Now.ToString()} {Environment.MachineName} " +
                 $"OS: {Environment.OSVersion.VersionString}";
            return new string[] { info };
        }
    }
}