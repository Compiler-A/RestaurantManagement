
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayerRestaurant.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Auth> Auths { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<StatusMenu> StatusMenus { get; set; }
        public DbSet<StatusOrder> StatusOrders { get; set; }
        public DbSet<StatusTable> StatusTables { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TypeItem> TypeItems { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }
    } 
}
