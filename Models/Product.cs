using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        public string? Name_khongdau { get; set; }

        [DisplayName("Giá tiền")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [DisplayName("Giá tiền gốc")]
        [Range(0, double.MaxValue)]
        public decimal OriginialPrice { get; set; }

        [DisplayName("Khuyến mãi")]
        [Range(0, 100, ErrorMessage = "Khuyến mãi phải từ 0 đến 100")]
        public int Promotion { get; set; }

        [DisplayName("Giảm giá (%)")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải từ 0 đến 100")]
        public int Discount { get; set; }

        [DisplayName("URL Hình ảnh")]
        [Required(ErrorMessage = "Vui lòng nhập URL hình ảnh")]
        public string ImageUrl { get; set; } = string.Empty;

        [DisplayName("Mô tả sản phẩm")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Loại Danh mục")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
    }

}