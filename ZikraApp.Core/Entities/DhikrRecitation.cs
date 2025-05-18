using System;

namespace ZikraApp.Core.Entities
{
    public class DhikrRecitation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DhikrId { get; set; }
        public DateTime RecitedAt { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Dhikr Dhikr { get; set; } = null!;
    }
} 