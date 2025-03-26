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

            // Seed danh mục
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hoa Hồng", Name_khongdau = "hoa hong" },
                new Category { Id = 2, Name = "Hoa Lan", Name_khongdau = "hoa lan" },
                new Category { Id = 3, Name = "Hoa Cúc", Name_khongdau = "hoa cuc" },
                new Category { Id = 4, Name = "Hoa Ly", Name_khongdau = "hoa ly" },
                new Category { Id = 5, Name = "Hoa Tulip", Name_khongdau = "hoa tulip" },
                new Category { Id = 6, Name = "Hoa Cẩm Chướng", Name_khongdau = "hoa cam chuong" }
            );

            // Seed sản phẩm
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Bó Hoa Hồng Đỏ", Name_khongdau = "bo hoa hong do", Price = 500000, OriginialPrice = 550000, Promotion = 10, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65", Description = "Hoa hồng đỏ tươi", CategoryId = 1 },
                new Product { Id = 2, Name = "Chậu Hoa Lan Trắng", Name_khongdau = "chau hoa lan trang", Price = 750000, OriginialPrice = 800000, Promotion = 15, Discount = 10, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518", Description = "Hoa lan trắng thanh khiết", CategoryId = 2 },
                new Product { Id = 3, Name = "Giỏ Hoa Cúc Vàng", Name_khongdau = "gio hoa cuc vang", Price = 300000, OriginialPrice = 320000, Promotion = 5, Discount = 2, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207", Description = "Hoa cúc vàng tươi sáng", CategoryId = 3 },
                new Product { Id = 4, Name = "Hoa Ly Hồng", Name_khongdau = "hoa ly hong", Price = 600000, OriginialPrice = 650000, Promotion = 12, Discount = 6, ImageUrl = "https://plus.unsplash.com/premium_photo-1688045685821-4958c1e28322", Description = "Hoa ly hồng nhẹ nhàng", CategoryId = 4 },
                new Product { Id = 5, Name = "Bó Hoa Tulip Đỏ", Name_khongdau = "bo hoa tulip do", Price = 800000, OriginialPrice = 850000, Promotion = 10, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1498998754966-9f72acbc85b2", Description = "Hoa tulip đỏ quyến rũ", CategoryId = 5 },
                new Product { Id = 6, Name = "Chậu Hoa Cẩm Chướng", Name_khongdau = "chau hoa cam chuong", Price = 450000, OriginialPrice = 470000, Promotion = 8, Discount = 3, ImageUrl = "https://images.unsplash.com/photo-1460039230329-eb070fc6c77c", Description = "Hoa cẩm chướng đẹp mắt", CategoryId = 6 }
            );

            // Seed hình ảnh sản phẩm
            modelBuilder.Entity<ProductImage>().HasData(
               new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65" },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518" },
                new ProductImage { Id = 3, ProductId = 2, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207" },
                new ProductImage { Id = 4, ProductId = 2, ImageUrl = "https://plus.unsplash.com/premium_photo-1688045685821-4958c1e28322" },
                new ProductImage { Id = 5, ProductId = 3, ImageUrl = "https://images.unsplash.com/photo-1498998754966-9f72acbc85b2" },
                new ProductImage { Id = 6, ProductId = 3, ImageUrl = "https://images.unsplash.com/photo-1460039230329-eb070fc6c77c" },
                new ProductImage { Id = 7, ProductId = 4, ImageUrl = "https://images.unsplash.com/photo-1468327768560-75b778cbb551" },
                new ProductImage { Id = 8, ProductId = 4, ImageUrl = "https://images.unsplash.com/photo-1444930694458-01babf71870c" },
                new ProductImage { Id = 9, ProductId = 5, ImageUrl = "https://images.unsplash.com/photo-1494972308805-463bc619d34e" },
                new ProductImage { Id = 10, ProductId = 5, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65" },
                new ProductImage { Id = 11, ProductId = 6, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518" },
                new ProductImage { Id = 12, ProductId = 6, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207" }
            );

            // // Seed người dùng
            // modelBuilder.Entity<ApplicationUser>().HasData(
            //     new ApplicationUser { Id = "1", UserName = "admin", FullName = "Quản trị viên", FullName_khongdau = "quan-tri-vien", Address = "Hà Nội", Age = "30", Email = "admin@example.com", NormalizedEmail = "ADMIN@EXAMPLE.COM", NormalizedUserName = "ADMIN" }
            // );

            // // Seed đơn hàng
            // modelBuilder.Entity<Order>().HasData(
            //     new Order { Id = 1, UserId = "1", OrderDate = DateTime.Now, TotalPrice = 1200000, Status = "Pending" }
            // );

            // // Seed chi tiết đơn hàng
            // modelBuilder.Entity<OrderDetail>().HasData(
            //     new OrderDetail { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, Price = 1000000 },
            //     new OrderDetail { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = 750000 }
            // );
        }

    }
}
