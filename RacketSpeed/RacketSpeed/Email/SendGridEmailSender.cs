using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;

namespace RacketSpeed.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridConfiguration _sendGridOptions;

        public SendGridEmailSender(IOptions<SendGridConfiguration> optionsAccessor)
        {
            _sendGridOptions = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(_sendGridOptions.ApiKey, subject, htmlMessage, email);
        }

        private Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
          
            var msg = MailHelper
                .CreateSingleEmail(
                from: new EmailAddress(_sendGridOptions.FromEmail, _sendGridOptions.FromName),
                to: new EmailAddress(email),
                subject, message, message);

            return client.SendEmailAsync(msg);
        }
    }
}

