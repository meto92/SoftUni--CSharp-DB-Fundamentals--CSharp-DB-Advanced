namespace SoftJail.Data
{
    using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
    {
        public SoftJailDbContext()
        { }

        public SoftJailDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Cell> Cells { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Mail> Mails { get; set; }

        public DbSet<Officer> Officers { get; set; }

        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }

        public DbSet<Prisoner> Prisoners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Prisoner>(entity =>
            {
                entity.HasOne(p => p.Cell)
                    .WithMany(c => c.Prisoners)
                    .HasForeignKey(p => p.CellId);

                entity.HasMany(p => p.Mails)
                    .WithOne(m => m.Prisoner)
                    .HasForeignKey(m => m.PrisonerId);
            });

            builder.Entity<Officer>(entity =>
            {
                entity.HasOne(o => o.Department)
                    .WithMany(d => d.Officers)
                    .HasForeignKey(o => o.DepartmentId);
            });

            builder.Entity<Cell>(entity =>
            {
                entity.HasOne(c => c.Department)
                    .WithMany(d => d.Cells)
                    .HasForeignKey(c => c.DepartmentId);

                entity.HasMany(c => c.Prisoners)
                    .WithOne(p => p.Cell)
                    .HasForeignKey(p => p.CellId);
            });

            builder.Entity<Mail>(entity =>
            {
                entity.HasOne(m => m.Prisoner)
                    .WithMany(p => p.Mails)
                    .HasForeignKey(m => m.PrisonerId);
            });

            builder.Entity<Department>(entity =>
            {
                entity.HasMany(d => d.Cells)
                    .WithOne(c => c.Department)
                    .HasForeignKey(c => c.DepartmentId);
            });

            builder.Entity<OfficerPrisoner>(entity =>
            {
                entity.HasKey(op => new { op.PrisonerId, op.OfficerId });

                entity.HasOne(op => op.Prisoner)
                    .WithMany(p => p.PrisonerOfficers)
                    .HasForeignKey(op => op.PrisonerId);

                entity.HasOne(op => op.Officer)
                    .WithMany(o => o.OfficerPrisoners)
                    .HasForeignKey(op => op.OfficerId);
            });
        }
    }
}