using FlowerShop.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

public class CartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "Cart";

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext.Session;

    public List<CartItem> GetCart()
    {
        var cartJson = Session.GetString(CartSessionKey);
        return string.IsNullOrEmpty(cartJson) 
            ? new List<CartItem>() 
            : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
    }

    public void SaveCart(List<CartItem> cart)
    {
        var cartJson = JsonSerializer.Serialize(cart);
        Session.SetString(CartSessionKey, cartJson);
    }

    public void AddToCart(Product product, int quantity)
    {
        var cart = GetCart();
        var existingItem = cart.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            });
        }

        SaveCart(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == productId);
        
        if (item != null)
        {
            if (quantity > 0)
                item.Quantity = quantity;
            else
                cart.Remove(item);
        }

        SaveCart(cart);
    }

    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == productId);
        
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }
    }

    public void ClearCart()
    {
        Session.Remove(CartSessionKey);
    }

    public decimal GetTotal()
    {
        return GetCart().Sum(item => item.Total);
    }
}