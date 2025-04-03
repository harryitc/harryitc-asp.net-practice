using System.ComponentModel;

namespace FlowerShop.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [DisplayName("URL Hình ảnh")]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [DisplayName("Hình ảnh chính")]
        public bool IsMainImage { get; set; } = false;
    }
}
