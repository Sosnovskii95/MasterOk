using Microsoft.EntityFrameworkCore;
using MasterOk.Models.ModelDataBase;

namespace MasterOk.Data
{
    public class DataBaseContext: DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ProductSold> ProductSolds { get; set; }

        public DbSet<ProductCheck> ProductChecks { get; set; }

        public DbSet<ShipToStore> ShipToStores { get; set; }

        public DbSet<DocShipToStore> DocShipToStores { get; set; }

        public DbSet<CartClient> CartClients { get; set; }

        public DbSet<PathImage> PathImages { get; set; }
    }
}
