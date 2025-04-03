using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlowerShop.Repository;
using System.Security.Claims;
using FlowerShop.Services;
using FlowerShop.Models;
using FlowerShop.Models.Payment;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FlowerShop.Controllers
{
    [Authorize] // Yêu cầu đăng nhập để đặt hàng
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly CartService _cartService;
        private readonly MoMoPaymentService _momoPaymentService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderRepository orderRepository,
            CartService cartService,
            MoMoPaymentService momoPaymentService,
            ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _momoPaymentService = momoPaymentService;
            _logger = logger;
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            // Thêm danh sách phương thức thanh toán
            ViewBag.PaymentMethods = new SelectList(
                Enum.GetValues(typeof(PaymentMethod))
                    .Cast<PaymentMethod>()
                    .Select(p => new { Id = (int)p, Name = GetDisplayName(p) }),
                "Id", "Name");

            return View();
        }

        // Hàm lấy tên hiển thị của enum
        private string GetDisplayName(PaymentMethod paymentMethod)
        {
            var fieldInfo = paymentMethod.GetType().GetField(paymentMethod.ToString());
            var displayAttributes = fieldInfo.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false) as
                System.ComponentModel.DataAnnotations.DisplayAttribute[];

            return displayAttributes.Length > 0 ? displayAttributes[0].Name : paymentMethod.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string shippingName, string shippingPhone, string shippingAddress, string note, PaymentMethod paymentMethod)
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
                PaymentMethod = paymentMethod,
                IsPaid = false,
                OrderDetails = cart.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            // Xử lý thanh toán dựa trên phương thức được chọn
            if (paymentMethod == PaymentMethod.MoMo)
            {
                try
                {
                    // Tạo yêu cầu thanh toán MoMo
                    var orderInfo = $"Đơn hàng {order.Id} - {order.ShippingName}";
                    var amount = (long)order.TotalPrice;
                    var orderId = $"{order.Id}_{DateTime.Now.Ticks}";

                    var momoResponse = await _momoPaymentService.CreatePaymentAsync(orderId, amount, orderInfo);

                    if (momoResponse.ErrorCode == 0)
                    {
                        // Lưu thông tin giao dịch
                        order.TransactionId = momoResponse.OrderId;
                        await _orderRepository.UpdateAsync(order);

                        // Chuyển hướng đến trang thanh toán MoMo
                        return Redirect(momoResponse.PayUrl);
                    }
                    else
                    {
                        _logger.LogError($"MoMo payment error: {momoResponse.Message}");
                        TempData["ErrorMessage"] = $"Lỗi thanh toán MoMo: {momoResponse.LocalMessage}";
                        return RedirectToAction("Details", new { id = order.Id });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"MoMo payment exception: {ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý thanh toán MoMo";
                    return RedirectToAction("Details", new { id = order.Id });
                }
            }
            else
            {
                // Thanh toán COD hoặc chuyển khoản
                _cartService.ClearCart();
                TempData["SuccessMessage"] = "Đặt hàng thành công!";
                return RedirectToAction("Details", new { id = order.Id });
            }
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



        // Xử lý callback từ MoMo
        [AllowAnonymous]
        public async Task<IActionResult> MoMoPaymentCallback(string partnerCode, string orderId, string requestId,
            long amount, string orderInfo, string orderType, string transId, string resultCode, string message,
            string payType, string responseTime, string extraData, string signature)
        {
            _logger.LogInformation($"MoMo callback received: OrderId={orderId}, ResultCode={resultCode}");

            try
            {
                // Tách lấy ID đơn hàng từ orderId (format: {orderId}_{timestamp})
                var orderIdParts = orderId.Split('_');
                if (orderIdParts.Length < 1)
                {
                    _logger.LogError($"Invalid order ID format: {orderId}");
                    return RedirectToAction("Index", "Home");
                }

                int orderIdValue;
                if (!int.TryParse(orderIdParts[0], out orderIdValue))
                {
                    _logger.LogError($"Cannot parse order ID: {orderIdParts[0]}");
                    return RedirectToAction("Index", "Home");
                }

                var order = await _orderRepository.GetByIdAsync(orderIdValue);
                if (order == null)
                {
                    _logger.LogError($"Order not found: {orderIdValue}");
                    return RedirectToAction("Index", "Home");
                }

                // Kiểm tra kết quả thanh toán
                if (resultCode == "0")
                {
                    // Thanh toán thành công
                    order.IsPaid = true;
                    order.PaymentDate = DateTime.Now;
                    order.TransactionId = transId;
                    await _orderRepository.UpdateAsync(order);

                    // Xóa giỏ hàng
                    _cartService.ClearCart();

                    TempData["SuccessMessage"] = "Thanh toán thành công!";
                }
                else
                {
                    // Thanh toán thất bại
                    TempData["ErrorMessage"] = $"Thanh toán thất bại: {message}";
                }

                return RedirectToAction("Details", new { id = order.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing MoMo callback: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        // Xử lý IPN (Instant Payment Notification) từ MoMo
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MoMoIPN([FromBody] MoMoIPNRequest request)
        {
            _logger.LogInformation($"MoMo IPN received: OrderId={request.OrderId}, ErrorCode={request.ErrorCode}");

            try
            {
                // Xác thực chữ ký
                bool isValidSignature = _momoPaymentService.ValidateIPNSignature(request);
                if (!isValidSignature)
                {
                    _logger.LogError("Invalid MoMo IPN signature");
                    return BadRequest(new { message = "Invalid signature" });
                }

                // Tách lấy ID đơn hàng
                var orderIdParts = request.OrderId.Split('_');
                if (orderIdParts.Length < 1 || !int.TryParse(orderIdParts[0], out int orderIdValue))
                {
                    _logger.LogError($"Invalid order ID format: {request.OrderId}");
                    return BadRequest(new { message = "Invalid order ID format" });
                }

                var order = await _orderRepository.GetByIdAsync(orderIdValue);
                if (order == null)
                {
                    _logger.LogError($"Order not found: {orderIdValue}");
                    return NotFound(new { message = "Order not found" });
                }

                // Kiểm tra kết quả thanh toán
                if (request.ErrorCode == "0")
                {
                    // Thanh toán thành công
                    order.IsPaid = true;
                    order.PaymentDate = DateTime.Now;
                    order.TransactionId = request.TransId;
                    await _orderRepository.UpdateAsync(order);

                    return Ok(new { message = "IPN processed successfully" });
                }
                else
                {
                    _logger.LogWarning($"MoMo payment failed: {request.Message}");
                    return Ok(new { message = "Payment failed but IPN processed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing MoMo IPN: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                    return NotFound();

                await _orderRepository.DeleteAsync(order.Id);
                TempData["SuccessMessage"] = "Đã xóa đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting order: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa đơn hàng!";
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Edit(int id, Order updatedOrder)
        {
            if (id != updatedOrder.Id)
                return NotFound();

            // Lấy đơn hàng hiện tại từ database
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                return NotFound();

            // Cập nhật thông tin đơn hàng
            existingOrder.ShippingName = updatedOrder.ShippingName;
            existingOrder.ShippingPhone = updatedOrder.ShippingPhone;
            existingOrder.ShippingAddress = updatedOrder.ShippingAddress;
            existingOrder.Note = updatedOrder.Note;
            existingOrder.Status = updatedOrder.Status;
            existingOrder.PaymentMethod = updatedOrder.PaymentMethod;
            existingOrder.IsPaid = updatedOrder.IsPaid;
            existingOrder.TransactionId = updatedOrder.TransactionId;

            // Cập nhật ngày thanh toán nếu trạng thái thanh toán thay đổi
            if (existingOrder.IsPaid && !existingOrder.PaymentDate.HasValue)
            {
                existingOrder.PaymentDate = DateTime.Now;
            }
            else if (!existingOrder.IsPaid)
            {
                existingOrder.PaymentDate = null;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepository.UpdateAsync(existingOrder);
                    TempData["SuccessMessage"] = "Cập nhật đơn hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật đơn hàng: " + ex.Message);
                }
            }
            return View(existingOrder);
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

