using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.PreTest.src.BuildingBlocks.Service.Governance
{
    /// <summary>
    /// 邮件助手
    /// </summary>
    public class MailHelper
    {
        public static void SendMail(EmailSettings settings, string content)
        {

            List<ConsulNotice> list = JsonConvert.DeserializeObject<List<ConsulNotice>>(content);
            if (list != null && list.Count > 0)
            {
                var emailBody = new StringBuilder("健康检查故障:\r\n");
                foreach (var noticy in list)
                {
                    emailBody.AppendLine($"--------------------------------------");
                    emailBody.AppendLine($"Node:{noticy.Node}");
                    emailBody.AppendLine($"Service ID:{noticy.ServiceID}");
                    emailBody.AppendLine($"Service Name:{noticy.ServiceName}");
                    emailBody.AppendLine($"Check ID:{noticy.CheckID}");
                    emailBody.AppendLine($"Check Name:{noticy.Name}");
                    emailBody.AppendLine($"Check Status:{noticy.Status}");
                    emailBody.AppendLine($"Check Output:{noticy.Output}");
                    emailBody.AppendLine($"--------------------------------------");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(settings.FromWho, settings.FromAccount));
                message.To.Add(new MailboxAddress(settings.ToWho, settings.ToAccount));

                message.Subject = settings.Subject;
                message.Body = new TextPart("plain") { Text = emailBody.ToString() };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(settings.SmtpServer, settings.SmtpPort, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(settings.AuthAccount, settings.AuthPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }

    }
}

public class EmailSettings
{
    public string FromWho { get; set; }

    public string FromAccount { get; set; }

    public string ToWho { get; set; }

    public string ToAccount { get; set; }

    public string Subject { get; set; }

    public string SmtpServer { get; set; }

    public int SmtpPort { get; set; }

    public string AuthAccount { get; set; }

    public string AuthPassword { get; set; }
}

public class ConsulNotice
{
    public string Node { get; set; }
    public string ServiceID { get; set; }
    public string ServiceName { get; set; }
    public string CheckID { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Output { get; set; }

}
