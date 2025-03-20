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
    }
}
