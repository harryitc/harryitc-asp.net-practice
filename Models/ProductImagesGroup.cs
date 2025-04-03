using System.Collections.Generic;

namespace FlowerShop.Models
{
    public class ProductImagesGroup
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<ProductImage> Images { get; set; }
        public int Count { get; set; }
    }
}
