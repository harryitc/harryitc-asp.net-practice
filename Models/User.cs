public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Cần hash khi lưu
    public string Role { get; set; } = "Customer"; // Admin hoặc Customer

    // Một User có thể có nhiều Orders
    public ICollection<Order>? Orders { get; set; }
}
