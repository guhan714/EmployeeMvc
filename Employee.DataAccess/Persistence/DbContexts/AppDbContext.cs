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
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        
                protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employees>().ToTable("Employees").HasIndex(u => u.Email)
            .IsUnique();
            modelBuilder.Entity<Employees>().HasIndex(a => a.PhoneNumber)
                .IsUnique();
            modelBuilder.Entity<Employees>().HasIndex(a => a.NationalIdNumber)
                .IsUnique();

      
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

    }
}

}
