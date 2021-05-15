using Classes;
using System.Data.Entity;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PharmacyEntities;Integrated Security=True")
        {
        }

        public virtual DbSet<Pharmacy> Pharmacy { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderInfo> ProductsOrder { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
    }
}
