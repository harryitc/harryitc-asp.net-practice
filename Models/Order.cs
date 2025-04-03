using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FlowerShop.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;

    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";

    // Thông tin giao hàng
    [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
    [DisplayName("Tên người nhận")]
    public string ShippingName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [DisplayName("Số điện thoại")]
    public string ShippingPhone { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
    [DisplayName("Địa chỉ giao hàng")]
    public string ShippingAddress { get; set; }

    [DisplayName("Ghi chú")]
    public string? Note { get; set; }

    // Navigation properties
    public ApplicationUser User { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
