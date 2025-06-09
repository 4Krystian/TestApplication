using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace BusinessLogic.Infrastructure
{
    public class EFDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> Options)
            : base(Options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ShopId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(d => d.Orders)
                    .WithOne(p => p.Shop)
                    .HasForeignKey(d => d.OrderId);
            }            
            );

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Town)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasMany(d => d.Orders)
                    .WithOne(p => p.Customer)
                    .HasForeignKey(d => d.OrderId);
            }
            );

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.EAN)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.PriceGross)
                    .IsRequired();

                entity.Property(e => e.PriceNet)
                    .IsRequired();

                entity.HasMany(d => d.OrderItems)
                    .WithOne(p => p.Item)
                    .HasForeignKey(d => d.ItemId);
            }
            );

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.ItemId)
                    .IsRequired();

                entity.Property(e => e.OrderId)
                    .IsRequired();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ItemId);
            }
            );

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.ShopId)
                    .IsRequired();

                entity.Property(e => e.CustomerId)
                    .IsRequired();

                entity.Property(e => e.PaymentMethodId)
                    .IsRequired();

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShopId);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId);

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentMethodId);

                entity.HasMany(d => d.OrderItems)
                    .WithOne(p => p.Order)
                    .HasForeignKey(d => d.OrderId);
            }
            );

        }
    }

    class Program : IDesignTimeDbContextFactory<BusinessLogic.Infrastructure.EFDbContext>
    {
        public EFDbContext CreateDbContext(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddJsonFile("appsettings.json");

            IConfiguration configuration = configurationBuilder.Build();

            string connectionString = configuration["ConnectionStrings:TestDatabase"];

            DbContextOptionsBuilder<EFDbContext> optionsBuilder = new DbContextOptionsBuilder<EFDbContext>()
                .UseSqlServer(connectionString);

            return new EFDbContext(optionsBuilder.Options);
        }
    }
}
