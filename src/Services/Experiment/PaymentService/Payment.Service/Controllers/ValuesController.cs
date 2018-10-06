using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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