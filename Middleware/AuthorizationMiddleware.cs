using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string path = context.Request.Path.ToString().ToLower();

        // Kiểm tra nếu truy cập trang quản trị mà không có quyền
        if (path.StartsWith("/admin") && context.Session.GetString("UserRole") != RoleUser.Admin)
        {
            context.Response.Redirect("/Auth/Unauthorized");
            return;
        }

        await _next(context);
    }
}
