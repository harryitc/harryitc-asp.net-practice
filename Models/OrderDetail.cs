public class OrderDetail
{
    public int Id { get; set; }

    // Khóa ngoại đến Order
    public int OrderId { get; set; }
    public Order Order { get; set; }

    // Khóa ngoại đến Product
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
