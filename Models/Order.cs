public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        // public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; 

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
