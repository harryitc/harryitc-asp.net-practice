using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [DisplayName("Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        public string? Name_khongdau { get; set; }

        // Một Category có nhiều Products
        public ICollection<Product>? Products { get; set; }
    }
}
