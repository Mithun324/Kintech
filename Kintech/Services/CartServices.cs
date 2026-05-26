using Kintech.Models;
using Kintech.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Kintech.Services;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartKey = "Cart";

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public List<CartItem> GetCart()
    {
        var cartJson = Session.GetString(CartKey);
        if (string.IsNullOrEmpty(cartJson))
            return [];

        return JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? []; // ✅ null safe
    }

    public void SaveCart(List<CartItem> cart)
    {
        Session.SetString(CartKey, JsonConvert.SerializeObject(cart));
    }

    public void ClearCart()
    {
        Session.Remove(CartKey);
    }

    public int GetCartCount()
    {
        return GetCart().Sum(x => x.Quantity);
    }
}