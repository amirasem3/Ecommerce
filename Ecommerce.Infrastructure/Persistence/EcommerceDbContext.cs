using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.RelationEntities;
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
    
    public DbSet<Manufacturer> Manufacturers { get; set; }
    
    //Relation DB set
    public DbSet<ManufacturerProduct> ManufacturerProducts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(e => e.Id).HasColumnType("uuid").HasMaxLength(24).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });
        //User's Attribute Constraints
        modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(user => user.PhoneNumber).IsUnique();
        //Role Attribute Constraints
        modelBuilder.Entity<Role>().HasIndex(role => role.Name).IsUnique();
        
        //Manufacturer Attribute Constraints
        modelBuilder.Entity<Manufacturer>().HasIndex(man => man.PhoneNumber).IsUnique();
        modelBuilder.Entity<Manufacturer>().HasIndex(man => man.Address).IsUnique();
        modelBuilder.Entity<Manufacturer>().HasIndex(man => man.Email).IsUnique();
        //Role-User Relationship (one-to-many)
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        
        //Manufacturer-Product Relations(N-N)
        modelBuilder.Entity<ManufacturerProduct>()
            .HasKey(mp => new { mp.ManufacturerId, mp.ProductId });
        modelBuilder.Entity<ManufacturerProduct>()
            .HasOne(mp => mp.Manufacturer)
            .WithMany(m => m.Products)
            .HasForeignKey(mp => mp.ManufacturerId);

        modelBuilder.Entity<ManufacturerProduct>()
            .HasOne(mp => mp.Product)
            .WithMany(p => p.Manufacturers)
            .HasForeignKey(mp => mp.ProductId);

    }
}