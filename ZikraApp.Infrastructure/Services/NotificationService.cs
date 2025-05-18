using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZikraApp.Core.Interfaces;

namespace ZikraApp.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendDhikrReminderAsync(Guid userId, string dhikrName)
        {
            _logger.LogInformation($"[MOCK] Push notification sent to user {userId} for dhikr: {dhikrName}");
            return Task.CompletedTask;
        }

        public Task SendAchievementNotificationAsync(Guid userId, string achievementName)
        {
            _logger.LogInformation($"[MOCK] Achievement notification sent to user {userId} for: {achievementName}");
            return Task.CompletedTask;
        }
    }
} 