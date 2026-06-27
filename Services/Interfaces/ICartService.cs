using TeretanaBackend.Dto;
using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartAsync(Cart cart);
        Task<Cart> AddCartAsync(Cart cart, string userName);
        Task<Cart> UpdateCartAsync(Cart cart, string userName);
        Task<string?> DeleteCart(Cart cart);
        Task<CartItemsResponse> AddCartWIthItemsAsync(CartItemsRequest cartItemsRequest);
        Task<CartItemsResponse> UpdateCartWithItemsAsync(CartItemsRequest cartItemsRequest);
        Task<CartItemsResponse> GetCartWithItemsAsync(Cart cart);
    }
}
