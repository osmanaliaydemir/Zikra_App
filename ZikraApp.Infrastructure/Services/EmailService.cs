using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZikraApp.Core.Interfaces;

namespace ZikraApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendDhikrReminderAsync(string email, string dhikrName)
        {
            // SMTP ayarlarını appsettings.json'dan al
            var smtpSection = _configuration.GetSection("Smtp");
            var host = smtpSection["Host"];
            var port = int.Parse(smtpSection["Port"] ?? "587");
            var user = smtpSection["User"];
            var pass = smtpSection["Pass"];
            var from = smtpSection["From"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(from))
            {
                _logger.LogWarning("SMTP ayarları eksik, email gönderilemiyor. Log only.");
                _logger.LogInformation($"[MOCK] Email sent to {email} for dhikr reminder: {dhikrName}");
                return;
            }

            var message = new MailMessage(from, email)
            {
                Subject = "Zikir Hatırlatma",
                Body = $"Bugünkü zikriniz: {dhikrName}",
                IsBodyHtml = false
            };
            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };
            await client.SendMailAsync(message);
            _logger.LogInformation($"[SMTP] Email sent to {email} for dhikr reminder: {dhikrName}");
        }

        public Task SendWelcomeEmailAsync(string email, string userName)
        {
            // Benzer şekilde SMTP ile hoşgeldin maili gönderebilirsin
            _logger.LogInformation($"[MOCK] Welcome email sent to {email} for user: {userName}");
            return Task.CompletedTask;
        }
    }
} 