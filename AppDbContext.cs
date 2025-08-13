using Microsoft.EntityFrameworkCore; //gives you access to EF Core features.
using WebApplication1.Models; //allows this file to access your Complaint model.

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Complaint>? Complaint { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tell EF Core to use the exact table name
            modelBuilder.Entity<Complaint>().ToTable("Complaint");
        }
    }
}
