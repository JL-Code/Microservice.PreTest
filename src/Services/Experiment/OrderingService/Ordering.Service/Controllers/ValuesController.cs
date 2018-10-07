using Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static int _count = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromServices] ILogger logger)
        {
            //模拟熔断
            //_count++;
            //if (_count <= 3)
            //{
            //    Thread.Sleep(5000);
            //}
            var info = $"ordering.service: {DateTime.Now.ToString()} {Environment.MachineName} " +
               $"OS: {Environment.OSVersion.VersionString}";
            logger.Info(info);
            return new string[] { info };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

    }
}
