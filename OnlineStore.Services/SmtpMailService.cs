using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class SmtpMailService : IMailService
    {
        public async Task Send(Message message)
        {
            using(var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = @"C:\Temp";

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
