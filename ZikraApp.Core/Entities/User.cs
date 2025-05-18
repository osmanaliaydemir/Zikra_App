using System;
using System.Collections.Generic;

namespace ZikraApp.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        // Navigation properties
        public ICollection<UserDhikr> UserDhikrs { get; set; }
        public ICollection<DhikrProgress> DhikrProgresses { get; set; }
        public ICollection<DhikrRecitation> DhikrRecitations { get; set; } = new List<DhikrRecitation>();
    }
} 