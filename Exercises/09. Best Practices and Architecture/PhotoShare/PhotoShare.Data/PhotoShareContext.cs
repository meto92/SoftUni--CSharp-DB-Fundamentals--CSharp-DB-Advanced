namespace PhotoShare.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Configuration;

    public class PhotoShareContext : DbContext
    { 
        public PhotoShareContext() { }

	    public PhotoShareContext(DbContextOptions options)
		    : base(options)
	    { }
        
        public DbSet<User> Users { get; set; }   
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AlbumRole> AlbumsRoles { get; set; }
        public DbSet<Town> Towns { get; set; }	
	    public DbSet<AlbumTag> AlbumsTags { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=Photoshare;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlbumConfig());
            modelBuilder.ApplyConfiguration(new AlbumRoleConfig());
            modelBuilder.ApplyConfiguration(new AlbumTagConfig());
            modelBuilder.ApplyConfiguration(new FriendshipConfig());
            modelBuilder.ApplyConfiguration(new PictureConfig());
            modelBuilder.ApplyConfiguration(new TagConfig());
            modelBuilder.ApplyConfiguration(new TownConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
        }
    }
}