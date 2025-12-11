using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_i_EF_Core_Hemuppgift.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_i_EF_Core_Hemuppgift
{
    public class ShopContext : DbContext
    {
        // Map all tables to the database
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderRow> OrderRows => Set<OrderRow>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<OrderSummary> OrderSummaries => Set<OrderSummary>();
        public DbSet<CustomerOrderCount> CustomerOrderCounts => Set<CustomerOrderCount>();
        public DbSet<ProductSale> ProductSales => Set<ProductSale>();

        // Use SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "shop.db");

            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        // OnModelCreating for ruleset when creating the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OrderSummary
            modelBuilder.Entity<OrderSummary>(e =>
            {
                e.HasNoKey(); // Saknar PK alltså har ingen primärnyckel

                e.ToView("OrderSummaryView"); // Kopplar tabellen mot SQLite
            });

            // CustomerOrderCount
            modelBuilder.Entity<CustomerOrderCount>(e =>
            {
                e.HasNoKey();

                e.ToView("CustomerOrderCountView");
            });

            // Product Sale
            modelBuilder.Entity<ProductSale>(e =>
            {
                e.HasNoKey();
                e.ToView("ProductSalesView");
            });

            modelBuilder.Entity<Customer>(e =>
            {
                // Set primary key
                e.HasKey(c => c.CustomerID);

                // Set rules for properties
                e.Property(c => c.Name)
                    .HasMaxLength(255).IsRequired();

                e.Property(c => c.Email)
                    .HasMaxLength(255).IsRequired();

                e.Property(c => c.City)
                    .HasMaxLength(255).IsRequired();

                e.Property(c => c.Phone);

                // Set UNIQUE-index for Email
                e.HasIndex(c => c.Email).IsUnique();

                // Customer has many orders
                e.HasMany(c => c.Orders);
            });

            modelBuilder.Entity<Order>(e =>
            {
                // Set primary key
                e.HasKey(o => o.OrderID);

                // Set rules for properties
                e.Property(o => o.OrderDate)
                    .IsRequired();

                e.Property(o => o.Status)
                    .IsRequired().HasMaxLength(100);

                e.Property(o => o.TotalAmount)
                    .IsRequired();

                // Foreign keys
                e.HasOne(o => o.Customer)
                                .WithMany(c => c.Orders)
                                .HasForeignKey(o => o.CustomerID)
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                e.HasMany(o => o.OrderRows);
            });

            modelBuilder.Entity<OrderRow>(e =>
            {
                // Set primary key
                e.HasKey(or => or.OrderRowID);

                // Set rules for properties
                e.Property(or => or.Quantity)
                    .IsRequired();

                e.Property(or => or.UnitPrice)
                    .IsRequired();

                // Foreign keys
                e.HasOne(or => or.Order)
                                    .WithMany(o => o.OrderRows)
                                    .HasForeignKey(or => or.OrderID)
                                    .OnDelete(DeleteBehavior.Cascade)
                                    .IsRequired();

                e.HasOne(or => or.Product)
                                    .WithMany(p => p.OrderRows)
                                    .HasForeignKey(or => or.ProductID)
                                    .OnDelete(DeleteBehavior.Restrict)
                                    .IsRequired();
            });

            modelBuilder.Entity<Product>(e =>
            {
                // Set primary key
                e.HasKey(p => p.ProductID);

                e.Property(p => p.Price)
                                    .IsRequired();

                // Foreign keys
                e.HasMany(p => p.OrderRows);
            });
        }
    }
}
