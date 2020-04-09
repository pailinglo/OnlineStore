using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OnlineStore.Models;
using Microsoft.AspNetCore.Hosting;

namespace OnlineStore.Services
{
    public class SmtpMailService : IMailService
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public SmtpMailService(IConfiguration configuration, IHostingEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }
        public async Task Send(Message message)
        {
            using(var smtp = new SmtpClient())
            {
                if (env.IsDevelopment())
                {
                    //for local testing:
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = @"C:\Temp";
                }
                else
                {
                    //Use configuration values from environment variables
                    //Production environment can't use user secrets
                    var userName = configuration["Smtp:UserName"];
                    var password = configuration["Smtp:Password"];

                    var credential = new NetworkCredential
                    {
                        UserName = userName,
                        Password = password
                    };
                    smtp.Credentials = credential;
                    smtp.Host = configuration["Smtp:Host"];
                    smtp.Port = Convert.ToInt32(configuration["Smtp:Port"]);
                    smtp.EnableSsl = true;                    
                }

                MailMessage mailMessage = new MailMessage
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    From = new MailAddress(message.From),
                    IsBodyHtml = message.IsHtml
                };
                foreach(var address in message.To)
                {
                    mailMessage.To.Add(address);
                }
                if (message.CC != null)
                {
                    foreach (var address in message.CC)
                    {
                        mailMessage.CC.Add(address);
                    }
                }
                await smtp.SendMailAsync(mailMessage);

            }
        }
    }
}
