using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FlowerShop.Models;
using FlowerShop.Repository;

namespace FlowerShop.Controllers;

public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // ĐĂNG KÝ
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User model)
    {
        if (_userRepository.GetUserByEmail(model.Email) != null)
        {
            ModelState.AddModelError("", "Email đã tồn tại.");
            return View(model);
        }

        model.Role = RoleUser.Customer; // Mặc định là khách hàng
        _userRepository.AddUser(model);

        return RedirectToAction("Login");
    }

    // ĐĂNG NHẬP
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);

        if (user == null || user.Password != password) // Nên dùng mã hóa mật khẩu
        {
            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return View();
        }

        // Lưu thông tin vào Session
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.FullName);
        HttpContext.Session.SetString("UserRole", user.Role);
        HttpContext.Session.SetString("Email", user.Email);

        return RedirectToAction("Index", "Home");
    }

    // ĐĂNG XUẤT
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

        // ĐĂNG NHẬP
    [HttpGet]
    public IActionResult Unauthorized()
    {
        return View();
    }
}
