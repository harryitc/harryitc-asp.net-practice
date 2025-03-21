using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Bỏ tiền tố AspNet của các bảng: mặc định
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // // Seed danh mục
            // modelBuilder.Entity<Category>().HasData(
            //     new Category { Id = 1, Name = "Hoa Hồng" },
            //     new Category { Id = 2, Name = "Hoa Lan" },
            //     new Category { Id = 3, Name = "Hoa Cúc" },
            //     new Category { Id = 4, Name = "Hoa Ly" },
            //     new Category { Id = 5, Name = "Hoa Tulip" },
            //     new Category { Id = 6, Name = "Hoa Cẩm Chướng" }
            // );

            // // Seed sản phẩm
            // modelBuilder.Entity<Product>().HasData(
            //     new Product { Id = 1, Name = "Bó Hoa Hồng Đỏ", Price = 500000, Promotion = 10, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65", Description = "Hoa hồng đỏ tươi", CategoryId = 1 },
            //     new Product { Id = 2, Name = "Chậu Hoa Lan Trắng", Price = 750000, Promotion = 15, Discount = 10, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518", Description = "Hoa lan trắng thanh khiết", CategoryId = 2 },
            //     new Product { Id = 3, Name = "Giỏ Hoa Cúc Vàng", Price = 300000, Promotion = 5, Discount = 2, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207", Description = "Hoa cúc vàng tươi sáng", CategoryId = 3 },
            //     new Product { Id = 4, Name = "Hoa Ly Hồng", Price = 600000, Promotion = 12, Discount = 6, ImageUrl = "https://plus.unsplash.com/premium_photo-1688045685821-4958c1e28322", Description = "Hoa ly hồng nhẹ nhàng", CategoryId = 4 },
            //     new Product { Id = 5, Name = "Bó Hoa Tulip Đỏ", Price = 800000, Promotion = 10, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1498998754966-9f72acbc85b2", Description = "Hoa tulip đỏ quyến rũ", CategoryId = 5 },
            //     new Product { Id = 6, Name = "Chậu Hoa Cẩm Chướng", Price = 450000, Promotion = 8, Discount = 3, ImageUrl = "https://images.unsplash.com/photo-1460039230329-eb070fc6c77c", Description = "Hoa cẩm chướng đẹp mắt", CategoryId = 6 }
            // );

            // // Seed hình ảnh sản phẩm
            // modelBuilder.Entity<ProductImage>().HasData(
            //     new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65" },
            //     new ProductImage { Id = 2, ProductId = 1, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518" },
            //     new ProductImage { Id = 3, ProductId = 2, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207" },
            //     new ProductImage { Id = 4, ProductId = 2, ImageUrl = "https://plus.unsplash.com/premium_photo-1688045685821-4958c1e28322" },
            //     new ProductImage { Id = 5, ProductId = 3, ImageUrl = "https://images.unsplash.com/photo-1498998754966-9f72acbc85b2" },
            //     new ProductImage { Id = 6, ProductId = 3, ImageUrl = "https://images.unsplash.com/photo-1460039230329-eb070fc6c77c" },
            //     new ProductImage { Id = 7, ProductId = 4, ImageUrl = "https://images.unsplash.com/photo-1468327768560-75b778cbb551" },
            //     new ProductImage { Id = 8, ProductId = 4, ImageUrl = "https://images.unsplash.com/photo-1444930694458-01babf71870c" },
            //     new ProductImage { Id = 9, ProductId = 5, ImageUrl = "https://images.unsplash.com/photo-1494972308805-463bc619d34e" },
            //     new ProductImage { Id = 10, ProductId = 5, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65" },
            //     new ProductImage { Id = 11, ProductId = 6, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518" },
            //     new ProductImage { Id = 12, ProductId = 6, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207" }
            // );

            // // Seed người dùng
            // modelBuilder.Entity<User>().HasData(
            //     new User { Id = 1, FullName = "Admin", Email = "admin@example.com", Password = "admin123", Role = "Admin" },
            //     new User { Id = 2, FullName = "Nguyễn Văn A", Email = "user1@example.com", Password = "user123", Role = "Customer" },
            //     new User { Id = 3, FullName = "Trần Thị B", Email = "user2@example.com", Password = "user123", Role = "Customer" }
            // );
        }
    }
}
