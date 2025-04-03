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
                if (tableName != null && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }

            // Seed danh mục
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hoa Hồng", Name_khongdau = "hoa hong" },
                new Category { Id = 2, Name = "Hoa Lan", Name_khongdau = "hoa lan" },
                new Category { Id = 3, Name = "Hoa Cúc", Name_khongdau = "hoa cuc" },
                new Category { Id = 4, Name = "Hoa Ly", Name_khongdau = "hoa ly" },
                new Category { Id = 5, Name = "Hoa Tulip", Name_khongdau = "hoa tulip" },
                new Category { Id = 6, Name = "Hoa Cẩm Chướng", Name_khongdau = "hoa cam chuong" },
                new Category { Id = 7, Name = "Hoa Đồng Tiền", Name_khongdau = "hoa dong tien" },
                new Category { Id = 8, Name = "Hoa Hướng Dương", Name_khongdau = "hoa huong duong" },
                new Category { Id = 9, Name = "Hoa Thủy Tiên", Name_khongdau = "hoa thuy tien" },
                new Category { Id = 10, Name = "Hoa Sen", Name_khongdau = "hoa sen" }
            );

            // Seed sản phẩm
            modelBuilder.Entity<Product>().HasData(
                // Hoa Hồng
                new Product { Id = 1, Name = "Bó Hoa Hồng Đỏ 20 Bông", Name_khongdau = "bo hoa hong do 20 bong", Price = 500000, OriginialPrice = 550000, Promotion = 10, Discount = 9, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65", Description = "Bó hoa hồng đỏ gồm 20 bông hồng Ecuador nhập khẩu, kèm hoa baby trắng và các loại lá phụ. Được gói bằng giấy kraft và ruy băng cao cấp.", CategoryId = 1 },
                new Product { Id = 2, Name = "Bó Hoa Hồng Pastel", Name_khongdau = "bo hoa hong pastel", Price = 650000, OriginialPrice = 700000, Promotion = 15, Discount = 7, ImageUrl = "https://images.unsplash.com/photo-1556807182-1a42c64b8283", Description = "Bó hoa hồng pastel gồm các loại hoa hồng màu nhạt: hồng phấn, trắng, vàng nhạt. Kèm theo hoa baby và các loại lá trang trí.", CategoryId = 1 },

                // Hoa Lan
                new Product { Id = 3, Name = "Chậu Hoa Lan Hồ Điệp Trắng", Name_khongdau = "chau hoa lan ho diep trang", Price = 750000, OriginialPrice = 800000, Promotion = 15, Discount = 6, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518", Description = "Chậu hoa lan hồ điệp trắng tinh khiết với 2 cành hoa, tổng cộng 15-18 bông. Chậu sứ cao cấp kèm đế gỗ.", CategoryId = 2 },
                new Product { Id = 4, Name = "Lan Mokara Vàng", Name_khongdau = "lan mokara vang", Price = 850000, OriginialPrice = 900000, Promotion = 10, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1567339982760-eebd3419f2e5", Description = "Bó hoa lan Mokara vàng rực rỡ gồm 3 cành, mỗi cành 7-9 bông. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 2 },

                // Hoa Cúc
                new Product { Id = 5, Name = "Giỏ Hoa Cúc Trắng", Name_khongdau = "gio hoa cuc trang", Price = 300000, OriginialPrice = 320000, Promotion = 5, Discount = 6, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207", Description = "Giỏ hoa cúc trắng tinh khiết, gồm 20 bông cúc trắng cỡ lớn và các loại hoa lá phụ. Giỏ mây tre tự nhiên.", CategoryId = 3 },
                new Product { Id = 6, Name = "Bó Hoa Cúc Đa Sắc", Name_khongdau = "bo hoa cuc da sac", Price = 350000, OriginialPrice = 380000, Promotion = 8, Discount = 8, ImageUrl = "https://images.unsplash.com/photo-1525310072745-f49212b5ac6d", Description = "Bó hoa cúc đa sắc gồm các loại cúc màu trắng, vàng, hồng và tím. Được gói bằng giấy kraft và ruy băng.", CategoryId = 3 },

                // Hoa Ly
                new Product { Id = 7, Name = "Bó Hoa Ly Trắng", Name_khongdau = "bo hoa ly trang", Price = 600000, OriginialPrice = 650000, Promotion = 12, Discount = 8, ImageUrl = "https://plus.unsplash.com/premium_photo-1688045685821-4958c1e28322", Description = "Bó hoa ly trắng tinh khiết gồm 5 cành ly, mỗi cành 3-4 nụ hoa. Kèm theo hoa baby và lá bạc. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 4 },
                new Product { Id = 8, Name = "Hoa Ly Hồng", Name_khongdau = "hoa ly hong", Price = 620000, OriginialPrice = 680000, Promotion = 10, Discount = 9, ImageUrl = "https://images.unsplash.com/photo-1468327768560-75b778cbb551", Description = "Bó hoa ly hồng nhẹ nhàng gồm 5 cành ly, mỗi cành 3-4 nụ hoa. Kèm theo hoa baby và lá bạc. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 4 },

                // Hoa Tulip
                new Product { Id = 9, Name = "Bó Hoa Tulip Đỏ", Name_khongdau = "bo hoa tulip do", Price = 800000, OriginialPrice = 850000, Promotion = 10, Discount = 6, ImageUrl = "https://images.unsplash.com/photo-1498998754966-9f72acbc85b2", Description = "Bó hoa tulip đỏ rực rỡ gồm 20 bông tulip đỏ nhập khẩu Hà Lan. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 5 },
                new Product { Id = 10, Name = "Bó Hoa Tulip Hỗn Hợp", Name_khongdau = "bo hoa tulip hon hop", Price = 850000, OriginialPrice = 900000, Promotion = 12, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1520219306100-ec69c7d8c72a", Description = "Bó hoa tulip hỗn hợp gồm 20 bông tulip đủ màu: đỏ, vàng, hồng, trắng. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 5 },

                // Hoa Cẩm Chướng
                new Product { Id = 11, Name = "Chậu Hoa Cẩm Chướng", Name_khongdau = "chau hoa cam chuong", Price = 450000, OriginialPrice = 470000, Promotion = 8, Discount = 4, ImageUrl = "https://images.unsplash.com/photo-1460039230329-eb070fc6c77c", Description = "Chậu hoa cẩm chướng đủ màu sắc: hồng, đỏ, trắng. Chậu sứ cao cấp kèm đế gỗ.", CategoryId = 6 },
                new Product { Id = 12, Name = "Bó Hoa Cẩm Chướng Hồng", Name_khongdau = "bo hoa cam chuong hong", Price = 400000, OriginialPrice = 430000, Promotion = 5, Discount = 7, ImageUrl = "https://images.unsplash.com/photo-1494972308805-463bc619d34e", Description = "Bó hoa cẩm chướng hồng nhẹ nhàng gồm 20 bông cẩm chướng hồng và các loại hoa lá phụ. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 6 },

                // Hoa Đồng Tiền
                new Product { Id = 13, Name = "Bó Hoa Đồng Tiền Đỏ", Name_khongdau = "bo hoa dong tien do", Price = 380000, OriginialPrice = 420000, Promotion = 10, Discount = 10, ImageUrl = "https://images.unsplash.com/photo-1561181286-d3fee3785613", Description = "Bó hoa đồng tiền đỏ rực rỡ gồm 15 bông đồng tiền đỏ cỡ lớn và các loại hoa lá phụ. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 7 },
                new Product { Id = 14, Name = "Giỏ Hoa Đồng Tiền Đa Sắc", Name_khongdau = "gio hoa dong tien da sac", Price = 420000, OriginialPrice = 450000, Promotion = 8, Discount = 7, ImageUrl = "https://images.unsplash.com/photo-1525310072745-f49212b5ac6d", Description = "Giỏ hoa đồng tiền đa sắc gồm 20 bông đồng tiền đủ màu: đỏ, vàng, hồng, trắng. Giỏ mây tre tự nhiên.", CategoryId = 7 },

                // Hoa Hướng Dương
                new Product { Id = 15, Name = "Bó Hoa Hướng Dương", Name_khongdau = "bo hoa huong duong", Price = 550000, OriginialPrice = 600000, Promotion = 15, Discount = 8, ImageUrl = "https://images.unsplash.com/photo-1551731409-43eb3e517a1a", Description = "Bó hoa hướng dương rực rỡ gồm 10 bông hướng dương cỡ lớn và các loại hoa lá phụ. Được gói bằng giấy kraft và ruy băng.", CategoryId = 8 },
                new Product { Id = 16, Name = "Giỏ Hoa Hướng Dương Mini", Name_khongdau = "gio hoa huong duong mini", Price = 500000, OriginialPrice = 530000, Promotion = 10, Discount = 6, ImageUrl = "https://images.unsplash.com/photo-1594663582551-5dce069fed9c", Description = "Giỏ hoa hướng dương mini gồm 15 bông hướng dương cỡ nhỏ và các loại hoa lá phụ. Giỏ mây tre tự nhiên.", CategoryId = 8 },

                // Hoa Thủy Tiên
                new Product { Id = 17, Name = "Bó Hoa Thủy Tiên Trắng", Name_khongdau = "bo hoa thuy tien trang", Price = 700000, OriginialPrice = 750000, Promotion = 12, Discount = 7, ImageUrl = "https://images.unsplash.com/photo-1509719662285-2c6cf8bf1944", Description = "Bó hoa thủy tiên trắng tinh khiết gồm 15 bông thủy tiên trắng và các loại hoa lá phụ. Được gói bằng giấy và ruy băng cao cấp.", CategoryId = 9 },
                new Product { Id = 18, Name = "Chậu Hoa Thủy Tiên Vàng", Name_khongdau = "chau hoa thuy tien vang", Price = 750000, OriginialPrice = 800000, Promotion = 10, Discount = 6, ImageUrl = "https://images.unsplash.com/photo-1551826152-d7248d6b8190", Description = "Chậu hoa thủy tiên vàng rực rỡ gồm 10 bông thủy tiên vàng. Chậu sứ cao cấp kèm đế gỗ.", CategoryId = 9 },

                // Hoa Sen
                new Product { Id = 19, Name = "Bó Hoa Sen Hồng", Name_khongdau = "bo hoa sen hong", Price = 650000, OriginialPrice = 700000, Promotion = 15, Discount = 7, ImageUrl = "https://images.unsplash.com/photo-1606083192040-7a2040f8b6bc", Description = "Bó hoa sen hồng thanh tao gồm 10 bông sen hồng và lá sen. Được gói bằng giấy sen và ruy băng.", CategoryId = 10 },
                new Product { Id = 20, Name = "Bình Hoa Sen Trắng", Name_khongdau = "binh hoa sen trang", Price = 680000, OriginialPrice = 720000, Promotion = 10, Discount = 5, ImageUrl = "https://images.unsplash.com/photo-1589994160839-163cd867cfe8", Description = "Bình hoa sen trắng tinh khiết gồm 5 bông sen trắng và lá sen. Bình thủy tinh cao cấp.", CategoryId = 10 }
            );

            // Seed hình ảnh sản phẩm
            modelBuilder.Entity<ProductImage>().HasData(
                // Hình ảnh cho sản phẩm 1 - Bó Hoa Hồng Đỏ 20 Bông
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://images.unsplash.com/photo-1578972497170-bfc780c65f65", IsMainImage = true },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "https://images.unsplash.com/photo-1548094990-c16ca90f1f0d" },
                new ProductImage { Id = 3, ProductId = 1, ImageUrl = "https://images.unsplash.com/photo-1582794543139-8ac9cb0f7b11" },

                // Hình ảnh cho sản phẩm 2 - Bó Hoa Hồng Pastel
                new ProductImage { Id = 4, ProductId = 2, ImageUrl = "https://images.unsplash.com/photo-1556807182-1a42c64b8283", IsMainImage = true },
                new ProductImage { Id = 5, ProductId = 2, ImageUrl = "https://images.unsplash.com/photo-1587314168485-3236d6710814" },
                new ProductImage { Id = 6, ProductId = 2, ImageUrl = "https://images.unsplash.com/photo-1596438459194-f275f413d6ff" },

                // Hình ảnh cho sản phẩm 3 - Chậu Hoa Lan Hồ Điệp Trắng
                new ProductImage { Id = 7, ProductId = 3, ImageUrl = "https://plus.unsplash.com/premium_photo-1676070095327-751d3f003518", IsMainImage = true },
                new ProductImage { Id = 8, ProductId = 3, ImageUrl = "https://images.unsplash.com/photo-1566896869584-6af7b1f560fd" },
                new ProductImage { Id = 9, ProductId = 3, ImageUrl = "https://images.unsplash.com/photo-1610765516875-1ab0e1c2c4a4" },

                // Hình ảnh cho sản phẩm 4 - Lan Mokara Vàng
                new ProductImage { Id = 10, ProductId = 4, ImageUrl = "https://images.unsplash.com/photo-1567339982760-eebd3419f2e5", IsMainImage = true },
                new ProductImage { Id = 11, ProductId = 4, ImageUrl = "https://images.unsplash.com/photo-1518895949257-7621c3c786d7" },

                // Hình ảnh cho sản phẩm 5 - Giỏ Hoa Cúc Trắng
                new ProductImage { Id = 12, ProductId = 5, ImageUrl = "https://images.unsplash.com/photo-1476209446441-5ad72f223207", IsMainImage = true },
                new ProductImage { Id = 13, ProductId = 5, ImageUrl = "https://images.unsplash.com/photo-1508610048659-a06b669e3321" },

                // Hình ảnh cho sản phẩm 6 - Bó Hoa Cúc Đa Sắc
                new ProductImage { Id = 14, ProductId = 6, ImageUrl = "https://images.unsplash.com/photo-1525310072745-f49212b5ac6d", IsMainImage = true },
                new ProductImage { Id = 15, ProductId = 6, ImageUrl = "https://images.unsplash.com/photo-1487530811176-3780de880c2d" },

                // Hình ảnh cho sản phẩm 7 - Bó Hoa Ly Trắng
                new ProductImage { Id = 16, ProductId = 7, ImageUrl = "https://plus.unsplash.com/premium_photo-1688045685821-4958c1e28322", IsMainImage = true },
                new ProductImage { Id = 17, ProductId = 7, ImageUrl = "https://images.unsplash.com/photo-1591710668263-bee1e9db2a26" },

                // Hình ảnh cho sản phẩm 8 - Hoa Ly Hồng
                new ProductImage { Id = 18, ProductId = 8, ImageUrl = "https://images.unsplash.com/photo-1468327768560-75b778cbb551", IsMainImage = true },
                new ProductImage { Id = 19, ProductId = 8, ImageUrl = "https://images.unsplash.com/photo-1444930694458-01babf71870c" },

                // Hình ảnh cho sản phẩm 9 - Bó Hoa Tulip Đỏ
                new ProductImage { Id = 20, ProductId = 9, ImageUrl = "https://images.unsplash.com/photo-1498998754966-9f72acbc85b2", IsMainImage = true },
                new ProductImage { Id = 21, ProductId = 9, ImageUrl = "https://images.unsplash.com/photo-1520219306100-ec69c7d8c72a" },

                // Hình ảnh cho sản phẩm 10 - Bó Hoa Tulip Hỗn Hợp
                new ProductImage { Id = 22, ProductId = 10, ImageUrl = "https://images.unsplash.com/photo-1520219306100-ec69c7d8c72a", IsMainImage = true },
                new ProductImage { Id = 23, ProductId = 10, ImageUrl = "https://images.unsplash.com/photo-1589994160839-163cd867cfe8" },

                // Hình ảnh cho sản phẩm 11 - Chậu Hoa Cẩm Chướng
                new ProductImage { Id = 24, ProductId = 11, ImageUrl = "https://images.unsplash.com/photo-1460039230329-eb070fc6c77c", IsMainImage = true },
                new ProductImage { Id = 25, ProductId = 11, ImageUrl = "https://images.unsplash.com/photo-1494972308805-463bc619d34e" },

                // Hình ảnh cho sản phẩm 12 - Bó Hoa Cẩm Chướng Hồng
                new ProductImage { Id = 26, ProductId = 12, ImageUrl = "https://images.unsplash.com/photo-1494972308805-463bc619d34e", IsMainImage = true },
                new ProductImage { Id = 27, ProductId = 12, ImageUrl = "https://images.unsplash.com/photo-1561181286-d3fee3785613" },

                // Hình ảnh cho sản phẩm 13 - Bó Hoa Đồng Tiền Đỏ
                new ProductImage { Id = 28, ProductId = 13, ImageUrl = "https://images.unsplash.com/photo-1561181286-d3fee3785613", IsMainImage = true },
                new ProductImage { Id = 29, ProductId = 13, ImageUrl = "https://images.unsplash.com/photo-1525310072745-f49212b5ac6d" },

                // Hình ảnh cho sản phẩm 14 - Giỏ Hoa Đồng Tiền Đa Sắc
                new ProductImage { Id = 30, ProductId = 14, ImageUrl = "https://images.unsplash.com/photo-1525310072745-f49212b5ac6d", IsMainImage = true },
                new ProductImage { Id = 31, ProductId = 14, ImageUrl = "https://images.unsplash.com/photo-1561181286-d3fee3785613" },

                // Hình ảnh cho sản phẩm 15 - Bó Hoa Hướng Dương
                new ProductImage { Id = 32, ProductId = 15, ImageUrl = "https://images.unsplash.com/photo-1551731409-43eb3e517a1a", IsMainImage = true },
                new ProductImage { Id = 33, ProductId = 15, ImageUrl = "https://images.unsplash.com/photo-1594663582551-5dce069fed9c" },

                // Hình ảnh cho sản phẩm 16 - Giỏ Hoa Hướng Dương Mini
                new ProductImage { Id = 34, ProductId = 16, ImageUrl = "https://images.unsplash.com/photo-1594663582551-5dce069fed9c", IsMainImage = true },
                new ProductImage { Id = 35, ProductId = 16, ImageUrl = "https://images.unsplash.com/photo-1551731409-43eb3e517a1a" },

                // Hình ảnh cho sản phẩm 17 - Bó Hoa Thủy Tiên Trắng
                new ProductImage { Id = 36, ProductId = 17, ImageUrl = "https://images.unsplash.com/photo-1509719662285-2c6cf8bf1944", IsMainImage = true },
                new ProductImage { Id = 37, ProductId = 17, ImageUrl = "https://images.unsplash.com/photo-1551826152-d7248d6b8190" },

                // Hình ảnh cho sản phẩm 18 - Chậu Hoa Thủy Tiên Vàng
                new ProductImage { Id = 38, ProductId = 18, ImageUrl = "https://images.unsplash.com/photo-1551826152-d7248d6b8190", IsMainImage = true },
                new ProductImage { Id = 39, ProductId = 18, ImageUrl = "https://images.unsplash.com/photo-1509719662285-2c6cf8bf1944" },

                // Hình ảnh cho sản phẩm 19 - Bó Hoa Sen Hồng
                new ProductImage { Id = 40, ProductId = 19, ImageUrl = "https://images.unsplash.com/photo-1606083192040-7a2040f8b6bc", IsMainImage = true },
                new ProductImage { Id = 41, ProductId = 19, ImageUrl = "https://images.unsplash.com/photo-1589994160839-163cd867cfe8" },

                // Hình ảnh cho sản phẩm 20 - Bình Hoa Sen Trắng
                new ProductImage { Id = 42, ProductId = 20, ImageUrl = "https://images.unsplash.com/photo-1589994160839-163cd867cfe8", IsMainImage = true },
                new ProductImage { Id = 43, ProductId = 20, ImageUrl = "https://images.unsplash.com/photo-1606083192040-7a2040f8b6bc" }
            );

            // // Seed đơn hàng
            // modelBuilder.Entity<Order>().HasData(
            //     new Order {
            //         Id = 1,
            //         UserId = "1",
            //         OrderDate = new DateTime(2023, 11, 15, 10, 30, 0),
            //         TotalPrice = 1150000,
            //         Status = "Completed",
            //         ShippingName = "Nguyễn Văn A",
            //         ShippingAddress = "123 Đường Lê Lợi, Quận 1, TP.HCM",
            //         ShippingPhone = "0901234567",
            //         Note = "Giao hàng trong giờ hành chính"
            //     },
            //     new Order {
            //         Id = 2,
            //         UserId = "2",
            //         OrderDate = new DateTime(2023, 11, 20, 15, 45, 0),
            //         TotalPrice = 850000,
            //         Status = "Shipping",
            //         ShippingName = "Trần Thị B",
            //         ShippingAddress = "456 Đường Nguyễn Huệ, Quận 3, TP.HCM",
            //         ShippingPhone = "0912345678",
            //         Note = "Gọi điện trước khi giao"
            //     },
            //     new Order {
            //         Id = 3,
            //         UserId = "3",
            //         OrderDate = new DateTime(2023, 11, 25, 9, 15, 0),
            //         TotalPrice = 1350000,
            //         Status = "Processing",
            //         ShippingName = "Lê Văn C",
            //         ShippingAddress = "789 Đường Võ Văn Tần, Quận 10, TP.HCM",
            //         ShippingPhone = "0923456789",
            //         Note = "Giao hàng vào cuối tuần"
            //     },
            //     new Order {
            //         Id = 4,
            //         UserId = "4",
            //         OrderDate = new DateTime(2023, 11, 28, 14, 0, 0),
            //         TotalPrice = 500000,
            //         Status = "Pending",
            //         ShippingName = "Phạm Thị D",
            //         ShippingAddress = "101 Đường Nguyễn Văn Linh, Quận 7, TP.HCM",
            //         ShippingPhone = "0934567890",
            //         Note = "Gọi điện trước khi giao"
            //     },
            //     new Order {
            //         Id = 5,
            //         UserId = "5",
            //         OrderDate = new DateTime(2023, 11, 30, 11, 30, 0),
            //         TotalPrice = 1800000,
            //         Status = "Pending",
            //         ShippingName = "Hoàng Văn E",
            //         ShippingAddress = "202 Đường Trần Hưng Đạo, Quận 5, TP.HCM",
            //         ShippingPhone = "0945678901",
            //         Note = "Giao hàng giờ hành chính"
            //     }
            // );

            // // Seed chi tiết đơn hàng
            // modelBuilder.Entity<OrderDetail>().HasData(
            //     // Chi tiết đơn hàng 1
            //     new OrderDetail { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, Price = 500000 },
            //     new OrderDetail { Id = 2, OrderId = 1, ProductId = 5, Quantity = 1, Price = 300000 },
            //     new OrderDetail { Id = 3, OrderId = 1, ProductId = 12, Quantity = 1, Price = 350000 },

            //     // Chi tiết đơn hàng 2
            //     new OrderDetail { Id = 4, OrderId = 2, ProductId = 2, Quantity = 1, Price = 650000 },
            //     new OrderDetail { Id = 5, OrderId = 2, ProductId = 6, Quantity = 1, Price = 200000 },

            //     // Chi tiết đơn hàng 3
            //     new OrderDetail { Id = 6, OrderId = 3, ProductId = 3, Quantity = 1, Price = 750000 },
            //     new OrderDetail { Id = 7, OrderId = 3, ProductId = 7, Quantity = 1, Price = 600000 },

            //     // Chi tiết đơn hàng 4
            //     new OrderDetail { Id = 8, OrderId = 4, ProductId = 5, Quantity = 1, Price = 300000 },
            //     new OrderDetail { Id = 9, OrderId = 4, ProductId = 13, Quantity = 1, Price = 200000 },

            //     // Chi tiết đơn hàng 5
            //     new OrderDetail { Id = 10, OrderId = 5, ProductId = 9, Quantity = 1, Price = 800000 },
            //     new OrderDetail { Id = 11, OrderId = 5, ProductId = 4, Quantity = 1, Price = 850000 },
            //     new OrderDetail { Id = 12, OrderId = 5, ProductId = 11, Quantity = 1, Price = 150000 }
            // );
        }

    }
}
