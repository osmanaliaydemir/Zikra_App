using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZikraApp.Core.Interfaces;
using ZikraApp.Core.Entities;

namespace ZikraApp.API.Services
{
    public class DhikrReminderService : BackgroundService
    {
        private readonly ILogger<DhikrReminderService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        public DhikrReminderService(
            ILogger<DhikrReminderService> logger,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            INotificationService notificationService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var currentTime = DateTime.UtcNow;
                    var today = DateTime.UtcNow.DayOfWeek.ToString();
                    var activeReminders = await _unitOfWork.Repository<UserDhikr>()
                        .FindAsync(ud => ud.IsActive &&
                                       ud.ReminderTime.HasValue &&
                                       (string.IsNullOrEmpty(ud.Days) || ud.Days.Contains(today)) &&
                                       ud.ReminderTime.Value.Hours == currentTime.Hour &&
                                       ud.ReminderTime.Value.Minutes == currentTime.Minute);

                    foreach (var reminder in activeReminders)
                    {
                        var user = await _unitOfWork.Repository<User>().GetByIdAsync(reminder.UserId);
                        var dhikr = await _unitOfWork.Repository<Dhikr>().GetByIdAsync(reminder.DhikrId);

                        if (user != null && dhikr != null)
                        {
                            // Email bildirimi gönder
                            await _emailService.SendDhikrReminderAsync(user.Email, dhikr.Name);

                            // Push notification gönder
                            await _notificationService.SendDhikrReminderAsync(user.Id, dhikr.Name);

                            _logger.LogInformation($"Reminder sent for user {user.Id} and dhikr {dhikr.Id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing dhikr reminders");
                }

                // Her dakika kontrol et
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
} 