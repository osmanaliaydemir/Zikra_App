using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZikraApp.Core.Interfaces;

namespace ZikraApp.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IConfiguration _configuration;
        public NotificationService(ILogger<NotificationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task SendDhikrReminderAsync(Guid userId, string dhikrName)
        {
            // Örnek: Firebase/OneSignal gibi bir servis ile push notification gönderimi
            var provider = _configuration["Notification:Provider"];
            if (provider == "Firebase")
            {
                // Firebase ile push gönderimi için gerekli kodu buraya ekleyin
                _logger.LogInformation($"[MOCK] (Firebase) Push notification sent to user {userId} for dhikr: {dhikrName}");
            }
            else if (provider == "OneSignal")
            {
                // OneSignal ile push gönderimi için gerekli kodu buraya ekleyin
                _logger.LogInformation($"[MOCK] (OneSignal) Push notification sent to user {userId} for dhikr: {dhikrName}");
            }
            else
            {
                _logger.LogInformation($"[MOCK] Push notification sent to user {userId} for dhikr: {dhikrName}");
            }
            return Task.CompletedTask;
        }

        public Task SendAchievementNotificationAsync(Guid userId, string achievementName)
        {
            _logger.LogInformation($"[MOCK] Achievement notification sent to user {userId} for: {achievementName}");
            return Task.CompletedTask;
        }
    }
} 