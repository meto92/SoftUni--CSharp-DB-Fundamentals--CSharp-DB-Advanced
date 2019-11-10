using Instagraph.Data.EntityConfigurations;
using Instagraph.Models;

using Microsoft.EntityFrameworkCore;

namespace Instagraph.Data
{
    public class InstagraphDbContext : DbContext
    {
        public InstagraphDbContext()
        { }

        public InstagraphDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserFollower> UsersFollowers { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionConfiguration.ConnectionString);

                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            void applyConfig<TEntity, TEntityConfiguration>()
                where TEntity : class
                where TEntityConfiguration : IEntityTypeConfiguration<TEntity>, new()
                => modelBuilder.ApplyConfiguration(new TEntityConfiguration());

            applyConfig<Comment, CommentConfiguration>();

            applyConfig<Picture, PictureConfiguration>();

            applyConfig<Post, PostConfiguration>();

            applyConfig<User, UserConfiguration>();

            applyConfig<UserFollower, UserFollowerConfiguration>();
        }
    }
}