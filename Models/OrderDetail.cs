using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; } = 1;

        [Required(ErrorMessage = "Vui lòng nhập giá tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn hoặc bằng 0")]
        [DisplayName("Giá tiền")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
    }
}
