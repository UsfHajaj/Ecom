using Ecom.Core.DTOs;
using Ecom.Core.Serviecs;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(EmailDTO email)
        {
            MimeMessage message =new ();

            message.From.Add(new MailboxAddress("My Ecom", _configuration["EmailSetting:From"]));
            message.Subject = email.Subject;
            message.To.Add(new MailboxAddress(email.To, email.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.Content
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(_configuration["EmailSetting:Smtp"],
                        int.Parse(_configuration["EmailSetting:Port"]),true);

                    await smtp.AuthenticateAsync(_configuration["EmailSetting:UserName"],
                       _configuration["EmailSetting:Password"]);

                    await smtp.SendAsync(message);
                }
                catch (Exception ex) 
                {
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
