public static class Constants
{
    public static readonly List<MenuItem> MenuItems = new List<MenuItem>
{
    new MenuItem { Title = "Trang chủ", Area="", Controller = "Home", Action = "Index" },
    new MenuItem { Title = "Sản phẩm", Area="", Controller = "Product", Action = "Index"},
    new MenuItem { Title = "Danh mục", Area="", Controller = "Category", Action = "Index", Role=RoleUser.Admin }, // Chỉ Admin
    new MenuItem { Title = "Hình ảnh", Area="", Controller = "ProductImage", Action = "Index", Role=RoleUser.Admin }, // Chỉ Admin
    new MenuItem { Title = "Quản lý đơn hàng", Area="", Controller = "Order", Action = "Index", Role=RoleUser.Admin }, // Chỉ Admin
    new MenuItem { Title = "Đơn hàng của tôi", Area="", Controller = "Order", Action = "MyOrders" } // Cho tất cả người dùng đã đăng nhập
};

    public static readonly string AUTHOR_NAME = "2280600358_PhanNgocCuong";

}