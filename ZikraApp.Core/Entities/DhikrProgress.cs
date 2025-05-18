using System;

namespace ZikraApp.Core.Entities
{
    public class DhikrProgress
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DhikrId { get; set; }
        public int Count { get; set; }
        public DateTime CompletedAt { get; set; }
        public string Notes { get; set; }
        
        // Navigation properties
        public User User { get; set; }
        public Dhikr Dhikr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 