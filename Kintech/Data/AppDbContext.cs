using Kintech.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kintech.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<DeliverySlot> DeliverySlots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ Decimal precision
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(o => o.Price)
            .HasPrecision(18, 2);

        // ✅ Store OrderStatus enum as string — prevents int conversion error
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // ✅ Category self-referencing relationship
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);  // ✅ prevent cascade delete issues

        // ✅ Category seed data
        modelBuilder.Entity<Category>().HasData(

            // Phone Accessories
            new Category { Id = 1, Name = "Phone Accessories" },
            new Category { Id = 2, Name = "Cases & Covers", ParentId = 1 },
            new Category { Id = 3, Name = "Screen Protectors", ParentId = 1 },
            new Category { Id = 4, Name = "Chargers & Cables", ParentId = 1 },
            new Category { Id = 5, Name = "Power Banks", ParentId = 1 },
            new Category { Id = 6, Name = "Phone Stands & Holders", ParentId = 1 },

            // Audio
            new Category { Id = 10, Name = "Audio" },
            new Category { Id = 11, Name = "Wireless Earbuds", ParentId = 10 },
            new Category { Id = 12, Name = "Headphones", ParentId = 10 },
            new Category { Id = 13, Name = "Bluetooth Speakers", ParentId = 10 },

            // Wearables
            new Category { Id = 20, Name = "Wearables" },
            new Category { Id = 21, Name = "Smartwatches", ParentId = 20 },
            new Category { Id = 22, Name = "Fitness Trackers", ParentId = 20 },

            // Computing
            new Category { Id = 30, Name = "Computing" },
            new Category { Id = 31, Name = "Laptops & Tablets", ParentId = 30 },
            new Category { Id = 32, Name = "Keyboards & Mice", ParentId = 30 },
            new Category { Id = 33, Name = "USB Hubs & Docks", ParentId = 30 },

            // Gaming
            new Category { Id = 40, Name = "Gaming" },
            new Category { Id = 41, Name = "Gaming Controllers", ParentId = 40 },
            new Category { Id = 42, Name = "Gaming Headsets", ParentId = 40 },

            // Smart Home
            new Category { Id = 50, Name = "Smart Home" },
            new Category { Id = 51, Name = "Smart Lighting", ParentId = 50 },
            new Category { Id = 52, Name = "Security Cameras", ParentId = 50 },

            // Photography
            new Category { Id = 60, Name = "Photography" },
            new Category { Id = 61, Name = "Tripods", ParentId = 60 },
            new Category { Id = 62, Name = "Ring Lights", ParentId = 60 },

            // Power & Connectivity
            new Category { Id = 70, Name = "Power & Connectivity" },
            new Category { Id = 71, Name = "Wireless Chargers", ParentId = 70 },

            // Storage & Protection
            new Category { Id = 80, Name = "Storage & Protection" },
            new Category { Id = 81, Name = "SSD & Hard Drives", ParentId = 80 },

            // Deals
            new Category { Id = 90, Name = "Deals" },
            new Category { Id = 91, Name = "Best Sellers", ParentId = 90 },
            new Category { Id = 92, Name = "New Arrivals", ParentId = 90 }
        );

        modelBuilder.Entity<DeliverySlot>().HasData(

    new DeliverySlot
    {
        Id = 1,
        TimeSlot = "9 AM - 12 PM",
        IsActive = true
    },

    new DeliverySlot
    {
        Id = 2,
        TimeSlot = "12 PM - 3 PM",
        IsActive = true
    },

    new DeliverySlot
    {
        Id = 3,
        TimeSlot = "3 PM - 6 PM",
        IsActive = true
    },

    new DeliverySlot
    {
        Id = 4,
        TimeSlot = "6 PM - 9 PM",
        IsActive = true
    }
);
    }
}