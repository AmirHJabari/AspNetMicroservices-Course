using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private EmailSettings _settings;
        private ILogger<EmailService> _logger;

        public EmailService(EmailSettings settings, ILogger<EmailService> logger)
        {
            this._settings = settings;
            this._logger = logger;
        }

        public async Task<bool> SendEmailAsync(Application.Models.Email email)
        {
            try
            {
                var client = new SendGridClient(_settings.ApiKey);

                var to = new EmailAddress(email.To);
                var from = new EmailAddress(_settings.FromAddress, _settings.FromName);

                var sendGridMessage = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);

                var response = await client.SendEmailAsync(sendGridMessage);

                if (response.IsSuccessStatusCode)
                    _logger.LogTrace("Email sent.");
                else
                    _logger.LogError("Failed to send an email");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to send an email.");
                throw;
            }
        }
    }
}
