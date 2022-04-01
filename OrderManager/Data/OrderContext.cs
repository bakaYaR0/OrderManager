using Microsoft.EntityFrameworkCore;
using OrderManager.Models;

namespace OrderManager.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<Provider> Providers { get; set; }
    }
}
