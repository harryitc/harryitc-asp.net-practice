using FlowerShop.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly IProductRepository _productRepository;

    public CartController(
        CartService cartService,
        IProductRepository productRepository)
    {
        _cartService = cartService;
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }

    [HttpGet]
    public IActionResult GetCart()
    {
        var cart = _cartService.GetCart();
        return Json(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return NotFound();

        _cartService.AddToCart(product, quantity);

        // Nếu là AJAX request, trả về Ok
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Ok();

        // Nếu không, chuyển hướng về trang giỏ hàng
        return RedirectToAction(nameof(Index));
    }

    public class AddToCartModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        _cartService.UpdateQuantity(productId, quantity);

        // Nếu là AJAX request, trả về Ok
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Ok();

        // Nếu không, chuyển hướng về trang giỏ hàng
        return RedirectToAction(nameof(Index));
    }

    public class UpdateQuantityModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart(int productId)
    {
        _cartService.RemoveFromCart(productId);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Checkout()
    {
        var cart = _cartService.GetCart();
        if (!cart.Any())
            return RedirectToAction(nameof(Index));

        return View();
    }
}
