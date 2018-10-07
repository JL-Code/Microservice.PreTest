using Core.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace Payment.Service.Controllers
{
    /// <summary>
    /// 默认
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        HttpClient _httpClient;
        string _gatewayUri;
        public ValuesController(HttpClient httpClient, IConfiguration cfg)
        {
            _gatewayUri = $"http://{cfg["Gateway:IPAddress"]}:{cfg["Gateway:Port"]}";
            _httpClient = httpClient;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromServices] ILogger logger)
        {
            var info = $"payment.service: {DateTime.Now.ToString()} {Environment.MachineName} " +
                 $"OS: {Environment.OSVersion.VersionString}";
            logger.Info(info);
            return new string[] { info };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("trace")]
        public ActionResult<IEnumerable<string>> Trace()
        {
            var info = $"payment.service: {DateTime.Now.ToString()} {Environment.MachineName} " +
                 $"OS: {Environment.OSVersion.VersionString}";
            var requestResult = _httpClient.GetStringAsync($"{_gatewayUri}/ordering/values").GetAwaiter().GetResult();
            return new string[] { info, requestResult };
        }

        /// <summary>
        /// 获取认证用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        [Authorize]
        public ActionResult GetUser()
        {
            var user = User.Claims.ToList();
            return Ok(user.ToString());
        }
    }
}