using FlowerShop.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class Product
    {
        public int Id { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Giá tiền")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int Promotion { get; set; }
        public int Discount { get; set; }

        [DisplayName("URL Hình ảnh")]
        public string ImageUrl { get; set; }

        [DisplayName("Miêu tả")]
        public string Description { get; set; }

        [DisplayName("Loại Danh mục")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
    }