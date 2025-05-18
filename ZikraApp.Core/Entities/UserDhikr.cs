using System;

namespace ZikraApp.Core.Entities
{
    public class UserDhikr
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DhikrId { get; set; }
        public int DailyTarget { get; set; }
        public TimeSpan? ReminderTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public User User { get; set; }
        public Dhikr Dhikr { get; set; }
    }
} 