using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hoa Sinh Nhật" },
                new Category { Id = 2, Name = "Hoa Cưới" },
                new Category { Id = 3, Name = "Hoa Chúc Mừng" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Admin", Email = "admin@flowershop.com", Password = "123456", Role = "Admin" },
                new User { Id = 2, FullName = "Nguyễn Văn A", Email = "user1@example.com", Password = "123456", Role = "Customer" },
                new User { Id = 3, FullName = "Trần Thị B", Email = "user2@example.com", Password = "123456", Role = "Customer" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Bó Hoa Hồng Đỏ", Price = 500000, ImageUrl = "/images/hoa1.jpg", Description = "Bó hoa hồng đỏ rực rỡ", CategoryId = 1 },
                new Product { Id = 2, Name = "Lẵng Hoa Cưới Trắng", Price = 1200000, ImageUrl = "/images/hoa2.jpg", Description = "Lẵng hoa cưới tuyệt đẹp", CategoryId = 2 },
                new Product { Id = 3, Name = "Bó Hoa Chúc Mừng", Price = 800000, ImageUrl = "/images/hoa3.jpg", Description = "Hoa chúc mừng sang trọng", CategoryId = 3 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
