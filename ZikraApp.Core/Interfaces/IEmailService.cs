namespace ZikraApp.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendDhikrReminderAsync(string email, string dhikrName);
        Task SendWelcomeEmailAsync(string email, string userName);
    }
} 