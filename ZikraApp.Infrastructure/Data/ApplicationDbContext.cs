using Microsoft.EntityFrameworkCore;
using ZikraApp.Core.Entities;

namespace ZikraApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Dhikr> Dhikrs { get; set; } = null!;
        public DbSet<DhikrRecitation> DhikrRecitations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Dhikr>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ArabicText).IsRequired();
                entity.Property(e => e.TurkishText).IsRequired();
                entity.Property(e => e.EnglishText).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Count).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<DhikrRecitation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RecitedAt).IsRequired();
                entity.HasOne(e => e.User)
                    .WithMany(u => u.DhikrRecitations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Dhikr)
                    .WithMany(d => d.Recitations)
                    .HasForeignKey(e => e.DhikrId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 