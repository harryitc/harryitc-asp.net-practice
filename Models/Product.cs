using System.ComponentModel;

public class Product
{
    public int Id { get; set; }

    [DisplayName("Tên sản phẩm")]
    public string Name { get; set; }

    [DisplayName("Giá tiền")]
    public decimal Price { get; set; }
    [DisplayName("URL Hình ảnh")]
    public string ImageUrl { get; set; }
    [DisplayName("Miêu tả")]
    public string Description { get; set; }

    // Khóa ngoại đến Category
    [DisplayName("Loại Danh mục")]
    public int CategoryId { get; set; }
    [DisplayName("Danh mục")]
    public Category? Category { get; set; }

    // Một Product có thể thuộc nhiều OrderDetails
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}
