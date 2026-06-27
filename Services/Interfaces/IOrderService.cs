using TeretanaBackend.Dto;
using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> AddOrderAsync(OrderRequest request);
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(OrderRequest request);
        Task<Order> UpdateOrderAsync(OrderRequest request);
        Task<string?> DeleteOrder(OrderRequest request);
        Task<OrderWithItemsResponse> GetOrderWithItemsAsync(OrderRequest request);
        Task<OrderWithItemsResponse> AddOrderWithItemsAsync(OrderWithItemsRequest request);
        Task<OrderWithItemsResponse> UpdateOrderWithItemsAsync(OrderWithItemsRequest request);
        Task<OrderPaymentResponse> OrderPaymentAsync(OrderPaymentRequest request);
        Task<OrderWithItemsResponse> AddOrderWithItemsFromCartIdAsync(CartOrderRequest request);
    }
}
