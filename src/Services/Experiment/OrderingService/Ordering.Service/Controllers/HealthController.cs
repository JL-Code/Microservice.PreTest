﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        /// <summary>
        /// 健康检查
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("支付服务健康且可用");
        }
    }
}