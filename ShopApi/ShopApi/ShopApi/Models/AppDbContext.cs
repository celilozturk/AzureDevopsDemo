using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ShopApi.Models;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var productList = new List<Product>
        {
            new Product { Id = 1, OrderId = 1 , Name="Product 1", Price=10},
            new Product { Id = 2, OrderId = 1 , Name="Product 2", Price=20},
            new Product { Id = 3, OrderId = 2 , Name="Product 3", Price=30},
            new Product { Id = 4, OrderId = 3 , Name="Product 4", Price=40}
        };
        modelBuilder.Entity<Product>().HasData(productList);
        var orderList = new List<Order>
        {
            new Order { Id = 1, Name="Order 1" },
            new Order { Id = 2, Name="Order 2" },
            new Order { Id = 3, Name="Order 3" }
        };
        modelBuilder.Entity<Order>().HasData(orderList);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
   
}
