using Microservice.PreTest.src.BuildingBlocks.Service.Governance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notice.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public NoticeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post()
        {
            #region 邮件发送

            var bytes = new byte[10240];
            var i = Request.Body.ReadAsync(bytes, 0, bytes.Length);
            var content = System.Text.Encoding.UTF8.GetString(bytes).Trim('\0');

            EmailSettings settings = new EmailSettings()
            {
                SmtpServer = Configuration["Email:SmtpServer"],
                SmtpPort = Convert.ToInt32(Configuration["Email:SmtpPort"]),
                AuthAccount = Configuration["Email:AuthAccount"],
                AuthPassword = Configuration["Email:AuthPassword"],
                ToWho = Configuration["Email:ToWho"],
                ToAccount = Configuration["Email:ToAccount"],
                FromWho = Configuration["Email:FromWho"],
                FromAccount = Configuration["Email:FromAccount"],
                Subject = Configuration["Email:Subject"]
            };

            MailHelper.SendMail(settings, content);

            #endregion

            return Ok();
        }
    }
}