

namespace BlogAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(){}
        public AppDbContext(DbContextOptions options):base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Blogs) // blog dataset
                .WithOne(e => e.User) // user reference from blog model
                .HasForeignKey(e => e.Userid) // user id reference from blog
                .IsRequired();

            // modelBuilder.Entity<Blog>()
            //     .HasOne(e => e.User) // user model
            //     .WithMany(e => e.Blogs) // list of blogs in user model
            //     .HasForeignKey(e => e.Userid) // user id reference from blog
            //     .IsRequired(); // a blog requires a user
        }
    }
}