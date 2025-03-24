public static class Constants
{
    public static readonly List<MenuItem> MenuItems = new List<MenuItem>
{
    new MenuItem { Title = "Trang chủ", Area="", Controller = "Home", Action = "Index" },
    new MenuItem { Title = "Sản phẩm", Area="", Controller = "Product", Action = "Index"},
    new MenuItem { Title = "Danh mục", Area="", Controller = "Category", Action = "Index", Role=RoleUser.Admin }, // Chỉ Admin
    new MenuItem { Title = "Hình ảnh", Area="", Controller = "ProductImage", Action = "Index", Role=RoleUser.Admin } // Chỉ Admin
};

    public static readonly string AUTHOR_NAME = "2280600358_PhanNgocCuong";

}