using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.Persistence;
public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Product>(entity =>
    //     {
    //         entity.Property(e => e.Id).HasMaxLength(24).IsRequired();
    //         entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
    //     });
    //     modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();
    //     
    //     // modelBuilder.Entity<Library>()
    //     //     .HasKey(ub => new { ub.UserId, ub.BookId });
    //     //
    //     // modelBuilder.Entity<Library>()
    //     //     .HasOne(ub => ub.User)
    //     //     .WithMany(u => u.UserBooks)
    //     //     .HasForeignKey(ub => ub.UserId);
    //     //
    //     // modelBuilder.Entity<Library>()
    //     //     .HasOne(ub => ub.Book)
    //     //     .WithMany(b => b.UserBooks)
    //     //     .HasForeignKey(ub => ub.BookId);
    //     //
    //     // modelBuilder.Entity<User>()
    //     //     .HasOne(u => u.Role)
    //     //     .WithMany(r => r.Users)
    //     //     .HasForeignKey(u => u.RoleId)
    //     //     .OnDelete(DeleteBehavior.Restrict);
    //
    // }
}