using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FlowerShop.Models;

namespace FlowerShop.Controllers;

public class BaseController : Controller
{
    public BaseController()
    {
        ViewData["MenuItems"] = Constants.MenuItems; // Gán menu vào ViewData
    }
}
