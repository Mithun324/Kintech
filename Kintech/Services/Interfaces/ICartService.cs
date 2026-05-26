using Kintech.Models;

namespace Kintech.Services.Interfaces;

public interface ICartService
{
    List<CartItem> GetCart();
    void SaveCart(List<CartItem> cart);
    void ClearCart();
    int GetCartCount();
}