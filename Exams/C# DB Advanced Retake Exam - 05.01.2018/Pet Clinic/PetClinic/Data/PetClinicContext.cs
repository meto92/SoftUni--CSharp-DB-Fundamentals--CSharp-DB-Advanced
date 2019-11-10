namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;
    using PetClinic.Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            :base(options) { }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<AnimalAid> AnimalAids { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }

        public DbSet<Vet> Vets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Animal>(e =>
            {
                e.HasKey(a => a.Id);

                e.Property(a => a.Name)
                .HasMaxLength(20)
                    .IsRequired();

                e.Property(a => a.Type)
                    .HasMaxLength(20)
                    .IsRequired();

                e.Property(a => a.Age);

                e.HasOne(a => a.Passport)
                    .WithOne(p => p.Animal)
                    .HasForeignKey<Animal>(a => a.PassportSerialNumber);

                e.HasMany(a => a.Procedures)
                    .WithOne(p => p.Animal)
                    .HasForeignKey(p => p.AnimalId);
            });

            builder.Entity<Passport>(e =>
            {
                e.HasKey(p => p.SerialNumber);
            });

            builder.Entity<Vet>(e =>
            {
                e.HasKey(v => v.Id);

                e.Property(v => v.Name)
                    .IsRequired();

                e.Property(v => v.Profession)
                    .IsRequired();

                e.HasIndex(v => v.PhoneNumber);

                e.HasMany(v => v.Procedures)
                    .WithOne(p => p.Vet)
                    .HasForeignKey(p => p.VetId);
            });

            builder.Entity<Procedure>(e =>
            {
                e.HasKey(p => p.Id);

                e.Ignore(p => p.Cost);

                e.HasOne(p => p.Animal)
                    .WithMany(a => a.Procedures)
                    .HasForeignKey(p => p.AnimalId);

                e.HasOne(p => p.Vet)
                    .WithMany(v => v.Procedures)
                    .HasForeignKey(p => p.VetId);
            });

            builder.Entity<AnimalAid>(e =>
            {
                e.HasKey(aa => aa.Id);

                e.HasIndex(aa => aa.Name);
            });

            builder.Entity<ProcedureAnimalAid>(e =>
            {
                e.HasKey(paa => new { paa.ProcedureId, paa.AnimalAidId });

                e.HasOne(paa => paa.Procedure)
                    .WithMany(p => p.ProcedureAnimalAids)
                    .HasForeignKey(paa => paa.ProcedureId);

                e.HasOne(paa => paa.AnimalAid)
                    .WithMany(aa => aa.AnimalAidProcedures)
                    .HasForeignKey(paa => paa.AnimalAidId);
            });
        }
    }
}