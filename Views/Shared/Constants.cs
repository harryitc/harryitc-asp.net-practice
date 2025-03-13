public static class Constants
{
    public static readonly List<MenuItem> MenuItems = new List<MenuItem>
    {
        new MenuItem { Title = "Trang chủ", Controller = "Home", Action = "Index" },
        new MenuItem { Title = "Sản phẩm", Controller = "Product", Action = "Index" },
        new MenuItem { Title = "Danh mục", Controller = "Category", Action = "Index" }
    };
    public static readonly string AUTHOR_NAME = "2280600358_PhanNgocCuong";
}