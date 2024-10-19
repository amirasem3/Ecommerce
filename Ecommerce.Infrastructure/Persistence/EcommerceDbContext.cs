using System.Data;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.RelationEntities;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.Persistence;
public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);


    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Invoice> Invoices { get; set; }
    
    public DbSet<Manufacturer> Manufacturers { get; set; }
    
    //Relation DB set
    public DbSet<ManufacturerProduct> ManufacturerProducts { get; set; }
    
    public DbSet<ProductInvoice> ProductInvoices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Product Model Rules
        // modelBuilder.Entity<Product>(entity =>
        // {
        //     entity.HasKey(p => p.Id);
        //     entity.Property(p => p.Id).HasColumnType("uuid").HasMaxLength(24).IsRequired();
        //     entity.Property(p => p.Price).HasColumnType("decimal(15,2)");
        //     entity.Property(p => p.Inventory).HasColumnType("decimal(15,2)");
        //     entity.Property(p => p.Name).HasMaxLength(50);
        // });
        
        //Manufacturer Model Rules
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasIndex(m => m.PhoneNumber).IsUnique();
            entity.HasIndex(m => m.Address).IsUnique();
            entity.HasIndex(m => m.Email).IsUnique();
            entity.Property(m => m.Name).HasMaxLength(20).IsRequired();
            entity.Property(m => m.OwnerName).HasMaxLength(50).IsRequired();
            entity.Property(m => m.ManufacturerCountry).HasMaxLength(60).IsRequired();
            entity.Property(m => m.Email).HasMaxLength(60).IsRequired();
            entity.Property(m => m.Address).HasMaxLength(100).IsRequired();
            entity.Property(m => m.PhoneNumber).HasMaxLength(20).IsRequired();

        });

        //User Model Rules
        // modelBuilder.Entity<User>(entity =>
        // {
        //     entity.HasIndex(u => u.Username).IsUnique();
        //     entity.HasIndex(u => u.Email).IsUnique();
        //     entity.HasIndex(u => u.PhoneNumber).IsUnique();
        //     entity.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
        //     entity.Property(u => u.LastName).HasMaxLength(50).IsRequired();
        //     entity.Property(u => u.Username).HasMaxLength(30).IsRequired();
        //     entity.Property(u => u.PasswordHash).HasMaxLength(90).IsRequired();
        //     entity.Property(u => u.Email).HasMaxLength(60).IsRequired();
        //     entity.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired();
        // });

        //Role Model Rules
        // modelBuilder.Entity<Role>(entity =>
        // {
        //     entity.HasIndex(r => r.Name).IsUnique();
        //     entity.Property(r => r.Name).HasMaxLength(40).IsRequired();
        // });

        //Category Model Rules
        // modelBuilder.Entity<Category>(entity =>
        // {
        //     entity.HasIndex(c => c.Name).IsUnique();
        //     entity.Property(c => c.Name).HasMaxLength(20).IsRequired();
        //     entity.Property(c => c.Type).IsRequired();
        //     entity.Property(c => c.ParentCategoryId).HasMaxLength(40).IsRequired();
        // });

        // modelBuilder.Entity<Invoice>(entity =>
        // {
        //     entity.HasIndex(i => i.IdentificationCode).IsUnique();
        // });
        //Invoice Attributes Constraints
        // modelBuilder.Entity<Invoice>()
        //     .HasIndex(i => i.IdentificationCode).IsUnique();

        modelBuilder.HasPostgresEnum<PaymentStatus>(name:"payment_status");
        modelBuilder.Entity<Invoice>()
            .Property(e => e.PaymentStatus)
            .HasConversion<string>()
            .HasColumnType("payment_status");
        
        
        //Manufacturer-Product Relations(Many-to-Many)
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
        
        //Product-Invoice (Many-to-Many)
        modelBuilder.Entity<ProductInvoice>()
            .HasKey(pi => new { pi.ProductId, pi.InvoiceId });

        modelBuilder.Entity<ProductInvoice>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.Invoices)
            .HasForeignKey(pi => pi.ProductId);

        modelBuilder.Entity<ProductInvoice>()
            .HasOne(pi => pi.Invoice)
            .WithMany(i => i.Products)
            .HasForeignKey(pi => pi.InvoiceId);
        
        
        //2nd try manufacturer and product many-to-many by convention
        modelBuilder.Entity<Manufacturer>()
            .HasMany(e => e.Products2)
            .WithMany(e => e.Manufacturers2);

        modelBuilder.Entity<Product>()
            .HasMany(e => e.Manufacturers2)
            .WithMany(e => e.Products2);


    }
}