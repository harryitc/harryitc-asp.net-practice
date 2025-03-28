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
    public async Task<IActionResult> AddToCart([FromBody] AddToCartModel model)
    {
        var product = await _productRepository.GetByIdAsync(model.ProductId);
        if (product == null) return NotFound();

        _cartService.AddToCart(product, model.Quantity);
        return Ok();
    }

    public class AddToCartModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity([FromBody] UpdateQuantityModel model)
    {
        _cartService.UpdateQuantity(model.ProductId, model.Quantity);
        return Ok();
    }

    public class UpdateQuantityModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    [HttpPost]
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
