using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FlowerShop.Models.Payment;

namespace FlowerShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";

        // Thông tin giao hàng
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
        [DisplayName("Tên người nhận")]
        public string ShippingName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [DisplayName("Số điện thoại")]
        public string ShippingPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [DisplayName("Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; } = string.Empty;

        [DisplayName("Ghi chú")]
        public string? Note { get; set; }

        // Thông tin thanh toán
        [DisplayName("Phương thức thanh toán")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.COD;

        [DisplayName("Mã giao dịch")]
        public string? TransactionId { get; set; }

        [DisplayName("Trạng thái thanh toán")]
        public bool IsPaid { get; set; } = false;

        [DisplayName("Ngày thanh toán")]
        public DateTime? PaymentDate { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
