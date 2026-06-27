using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class CartItemsResponse
    {
        public Cart Cart { get; set; } = null!;
        public List<CartItem> CartItems { get; set; } = null!;
    }
}
