﻿public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;

    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    
    // Thêm các trường mới
    public string ShippingName { get; set; }
    public string ShippingPhone { get; set; }
    public string ShippingAddress { get; set; }
    public string? Note { get; set; }

    // Navigation properties
    public ApplicationUser User { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
