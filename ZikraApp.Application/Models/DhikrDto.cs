using System;
using System.ComponentModel.DataAnnotations;

namespace ZikraApp.Application.Models
{
    public class DhikrDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ArabicText { get; set; }
        public string? TurkishText { get; set; }
        public string? EnglishText { get; set; }
        public string? Description { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastRecitedAt { get; set; }
    }

    public class CreateDhikrDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ArabicText { get; set; } = string.Empty;

        [Required]
        public string TurkishText { get; set; } = string.Empty;

        [Required]
        public string EnglishText { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }

    public class UpdateDhikrDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ArabicText { get; set; } = string.Empty;

        [Required]
        public string TurkishText { get; set; } = string.Empty;

        [Required]
        public string EnglishText { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }

    public class DhikrRecitationDto
    {
        public Guid Id { get; set; }
        public DhikrDto Dhikr { get; set; } = new();
        public DateTime RecitedAt { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateDhikrRecitationDto
    {
        [Required]
        public Guid DhikrId { get; set; }

        public string? Notes { get; set; }
    }

    public class UpdateDhikrRecitationDto
    {
        [Required]
        public string Notes { get; set; } = string.Empty;
    }

    public class UserDhikrDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DhikrId { get; set; }
        public int DailyTarget { get; set; }
        public TimeSpan? ReminderTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DhikrDto Dhikr { get; set; }
    }

    public class CreateUserDhikrDto
    {
        public Guid DhikrId { get; set; }
        public int DailyTarget { get; set; }
        public TimeSpan? ReminderTime { get; set; }
    }

    public class UpdateUserDhikrDto
    {
        public int DailyTarget { get; set; }
        public TimeSpan? ReminderTime { get; set; }
        public bool IsActive { get; set; }
    }

    public class DhikrProgressDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DhikrId { get; set; }
        public int Count { get; set; }
        public DateTime CompletedAt { get; set; }
        public string Notes { get; set; }
        public DhikrDto Dhikr { get; set; }
    }

    public class CreateDhikrProgressDto
    {
        public Guid DhikrId { get; set; }
        public int Count { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateDhikrProgressDto
    {
        public int Count { get; set; }
        public string Notes { get; set; }
    }
} 