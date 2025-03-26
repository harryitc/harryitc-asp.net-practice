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

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return NotFound();

        _cartService.AddToCart(product, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        _cartService.UpdateQuantity(productId, quantity);
        return RedirectToAction(nameof(Index));
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