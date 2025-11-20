using Microsoft.EntityFrameworkCore;
using CMCS.Models;

namespace CMCS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.LecturerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HoursWorked)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.HourlyRate)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Pending");

                entity.Property(e => e.SubmissionDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.Comments)
                    .HasMaxLength(500);

                entity.Property(e => e.ReviewedBy)
                    .HasMaxLength(100);

                entity.Property(e => e.LecturerEmail)
                    .HasMaxLength(100);

                entity.Property(e => e.Department)
                    .HasMaxLength(100);

                entity.Property(e => e.ModuleCode)
                    .HasMaxLength(20);

                entity.Property(e => e.ClaimMonth)
                    .HasMaxLength(50);

                entity.Ignore(e => e.TotalAmount);

                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.SubmissionDate);
                entity.HasIndex(e => e.LecturerName);
            });
        }
    }
}
