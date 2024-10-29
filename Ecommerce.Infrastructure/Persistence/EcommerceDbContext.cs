using Ecommerce.Core.Entities;
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
    
    public DbSet<ProductInvoice> ProductInvoices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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
        

        modelBuilder.HasPostgresEnum<PaymentStatus>(name:"payment_status");
        modelBuilder.Entity<Invoice>()
            .Property(e => e.PaymentStatus)
            .HasConversion<string>()
            .HasColumnType("payment_status");
        
        
        //Manufacturer-Product Relations(Many-to-Many)
        modelBuilder.Entity<Manufacturer>()
            .HasMany(m => m.Products2)
            .WithMany(p => p.Manufacturers2)
            .UsingEntity(j => j.ToTable("ManufacturerProduct2"));
        
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
        


    }
}