﻿using Ecommerce.Core.Entities;
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
    
    //Relation DB sets
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<ManufacturerProduct> ManufacturerProducts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(24).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });
        modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(user => user.PhoneNumber).IsUnique();

        modelBuilder.Entity<Role>().HasIndex(role => role.Name).IsUnique();
        
        //User-Role Relation(N-N)
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);    
        
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