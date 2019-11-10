using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        { }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(e =>
            {
                e.HasKey(s => s.StudentId);

                e.Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsUnicode();

                e.Property(s => s.PhoneNumber)
                    .IsUnicode(false)
                    .IsRequired(false);

                //e.Property(s => s.PhoneNumber)
                //    .HasColumnType("CHAR(10)");

                e.HasMany(s => s.HomeworkSubmissions)
                    .WithOne(h => h.Student)
                    .HasForeignKey(h => h.StudentId);
            });

            modelBuilder.Entity<Course>(e =>
            {
                e.HasKey(c => c.CourseId);

                e.Property(c => c.Name)
                    .HasMaxLength(80)
                    .IsUnicode();

                e.Property(c => c.Description)
                    .IsUnicode()
                    .IsRequired(false);

                e.HasMany(c => c.Resources)
                    .WithOne(r => r.Course)
                    .HasForeignKey(c => c.CourseId);

                e.HasMany(c => c.HomeworkSubmissions)
                    .WithOne(h => h.Course)
                    .HasForeignKey(h => h.CourseId);
            });

            modelBuilder.Entity<Resource>(e =>
            {
                e.HasKey(r => r.ResourceId);

                e.Property(r => r.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                e.Property(r => r.Url)
                    .IsUnicode(false);

                e.HasOne(r => r.Course)
                    .WithMany(c => c.Resources)
                    .HasForeignKey(r => r.CourseId);

                e.HasOne(r => r.Course)
                    .WithMany(c => c.Resources)
                    .HasForeignKey(r => r.CourseId);
            });

            modelBuilder.Entity<Homework>(e =>
            {
                e.HasKey(h => h.HomeworkId);

                e.Property(h => h.Content)
                    .IsUnicode(false);

                e.HasOne(h => h.Student)
                    .WithMany(s => s.HomeworkSubmissions)
                    .HasForeignKey(h => h.StudentId);

                e.HasOne(h => h.Course)
                    .WithMany(c => c.HomeworkSubmissions)
                    .HasForeignKey(h => h.CourseId);
            });

            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey(sc => new { sc.StudentId, sc.CourseId });

                e.HasOne(sc => sc.Student)
                    .WithMany(s => s.CourseEnrollments)
                    .HasForeignKey(sc => sc.StudentId);

                e.HasOne(sc => sc.Course)
                    .WithMany(c => c.StudentsEnrolled)
                    .HasForeignKey(sc => sc.CourseId);
            });
        }
    }
}