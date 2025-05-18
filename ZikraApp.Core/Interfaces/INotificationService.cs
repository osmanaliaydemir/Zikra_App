namespace ZikraApp.Core.Interfaces
{
    public interface INotificationService
    {
        Task SendDhikrReminderAsync(Guid userId, string dhikrName);
        Task SendAchievementNotificationAsync(Guid userId, string achievementName);
    }
} 