using Microsoft.EntityFrameworkCore;
using MasterOk.Models.ModelDataBase;

namespace MasterOk.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ProductSold> ProductSolds { get; set; }

        public DbSet<ProductCheck> ProductChecks { get; set; }

        public DbSet<ShipToStore> ShipToStores { get; set; }

        public DbSet<DocShipToStore> DocShipToStores { get; set; }

        public DbSet<CartClient> CartClients { get; set; }

        public DbSet<PathImage> PathImages { get; set; }

        public DbSet<PayMethod> PayMethods { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<StateOrder> StateOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PayMethod>().HasData(new List<PayMethod> { new PayMethod { Id = 1, TitlePayMethod = "Наличные" }, new PayMethod { Id = 2, TitlePayMethod = "Карта" } });
            modelBuilder.Entity<DeliveryMethod>().HasData(new List<DeliveryMethod> { new DeliveryMethod { Id = 1, TitleDeliveryMethod = "Самовывоз" }, new DeliveryMethod { Id = 2, TitleDeliveryMethod = "Доставка" } });
            modelBuilder.Entity<StateOrder>().HasData(new List<StateOrder> { new StateOrder { Id = 1, TitleState = "В обработке" }, new StateOrder { Id = 2, TitleState = "Одобрен" }, new StateOrder { Id = 3, TitleState = "Завершен" } });
            modelBuilder.Entity<User>().HasData(new User { LoginUser = "admin", EmailUser = "admin@admin", PasswordUser = "admin", ActiveUser = true, FirstLastNameStaff = "Администратор", NumberPhoneStaff = 375333333, Age = 27, Id=1 });
        }
    }
}
