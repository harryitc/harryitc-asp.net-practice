public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } // Một đơn hàng thuộc về một User

    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Completed, Canceled

    // Một Order có nhiều OrderDetails
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}
