using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Persistence.DbContexts
{

    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Employees> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
                protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employees>().ToTable("Employees").HasIndex(u => u.Email)
            .IsUnique();
            modelBuilder.Entity<Employees>().HasIndex(a => a.PhoneNumber)
                .IsUnique();
            modelBuilder.Entity<Employees>().HasIndex(a => a.NationalIdNumber)
                .IsUnique();

        }
    }

}
