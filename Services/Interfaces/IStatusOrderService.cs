using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface IStatusOrderService
    {
        Task<List<StatusOrder>> GetAllStatusOrdersAsync();
        Task<StatusOrder> GetStatusOrderAsync(StatusOrder statusOrder);
        Task<StatusOrder> AddStatusOrderAsync(StatusOrder statusOrder, string userName);
        Task<StatusOrder> UpdateStatusOrderAsync(StatusOrder statusOrder, string userName);
        Task<string?> DeleteStatusOrder(StatusOrder statusOrder);
    }
}
