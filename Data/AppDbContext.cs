namespace BlogAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(){}
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Blogs) // blog dataset
                .WithOne(e => e.User) // user model
                .HasForeignKey(e => e.Userid) // from blog
                .IsRequired(false);
        }
    }
}