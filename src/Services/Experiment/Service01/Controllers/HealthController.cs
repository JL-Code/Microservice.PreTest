using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Service01.Controllers
{
    /// <summary>
    /// consul 健康检查
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
       [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}