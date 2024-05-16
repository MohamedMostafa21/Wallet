using Digital_Wallet.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital_Wallet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
           .HasOne(c => c.Category)
           .WithMany()
           .HasForeignKey(c => c.CategoryId)
           .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
