using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlowerShop.Repository;
using System.Security.Claims;
using FlowerShop.Services;
using FlowerShop.Models;

namespace FlowerShop.Controllers
{
    [Authorize] // Yêu cầu đăng nhập để đặt hàng
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly CartService _cartService;

        public OrderController(
            IOrderRepository orderRepository,
            CartService cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
                return RedirectToAction("Index", "Cart");
                
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string shippingName, string shippingPhone, string shippingAddress, string note)
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                ShippingName = shippingName,
                ShippingPhone = shippingPhone,
                ShippingAddress = shippingAddress,
                Note = note,
                TotalPrice = cart.Sum(i => i.Total),
                OrderDetails = cart.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            _cartService.ClearCart();

            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("Details", new { id = order.Id });
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return View(order);
        }

        // GET: Order/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return View(orders);
        }

        // Admin: Quản lý đơn hàng
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            await _orderRepository.DeleteAsync(order.Id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _orderRepository.UpdateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Order/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            // Chỉ cho phép hủy đơn hàng khi đang ở trạng thái Pending
            if (order.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Không thể hủy đơn hàng ở trạng thái này!";
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }

            order.Status = "Cancelled";
            await _orderRepository.UpdateAsync(order);

            TempData["SuccessMessage"] = "Đã hủy đơn hàng thành công!";
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }
    }
}

