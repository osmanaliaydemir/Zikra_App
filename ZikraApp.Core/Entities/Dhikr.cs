using System;
using System.Collections.Generic;

namespace ZikraApp.Core.Entities
{
    public class Dhikr
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ArabicText { get; set; } = null!;
        public string TurkishText { get; set; } = null!;
        public string EnglishText { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastRecitedAt { get; set; }
        
        // Navigation properties
        public ICollection<DhikrRecitation> Recitations { get; set; } = new List<DhikrRecitation>();
    }
} 