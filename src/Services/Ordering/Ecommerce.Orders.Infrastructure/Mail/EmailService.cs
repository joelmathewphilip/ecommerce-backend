using Ecommerce.Orders.Application.Contracts.Infrastructure;
using Ecommerce.Orders.Application.Models;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders.Infrastructure.Mail
{
    public class EmailService : IEmailServices
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger;
        public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var from = new EmailAddress() { Email = _emailSettings.FromAddress, Name = _emailSettings.FromName };
            var to = new EmailAddress() { Email = email.To };
            var body = email.Body;
            var subject = email.Subject;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, htmlContent);
            var response = await client.SendEmailAsync(msg);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation("Email is sent");
                return true;
            }
            else
            {
                _logger.LogError("Email could not be sent");
                return false;

            }

        }
    }
}
