using System.ComponentModel;

public class Category
{
    public int Id { get; set; }
    
    [DisplayName("Tên danh mục")]
    public string Name { get; set; }

    // Một Category có nhiều Products
    public ICollection<Product>? Products { get; set; }
}
