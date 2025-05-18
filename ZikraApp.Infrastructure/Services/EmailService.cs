using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZikraApp.Core.Interfaces;

namespace ZikraApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendDhikrReminderAsync(string email, string dhikrName)
        {
            _logger.LogInformation($"[MOCK] Email sent to {email} for dhikr reminder: {dhikrName}");
            return Task.CompletedTask;
        }

        public Task SendWelcomeEmailAsync(string email, string userName)
        {
            _logger.LogInformation($"[MOCK] Welcome email sent to {email} for user: {userName}");
            return Task.CompletedTask;
        }
    }
} 