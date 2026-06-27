using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class StatusOrderService : IStatusOrderService
    {
        private readonly AppDbContext dbContext;

        public StatusOrderService(AppDbContext dbContext)
        {
            this.dbContext=dbContext;
        }

        public async Task<StatusOrder> AddStatusOrderAsync(StatusOrder statusOrder, string userName)
        {
            var statusOrderExists = await dbContext.StatusOrders.FirstOrDefaultAsync(so=>so.StatusDescrption == statusOrder.StatusDescrption);

            if (statusOrderExists is not null) throw new Exception("Given Status Order already exists");

            var newStatusOrder= new StatusOrder()
            {
                StatusDescrption = statusOrder.StatusDescrption,
                CreatedBy = userName,
                CreatedAt = DateTime.Now,
            };

            await dbContext.StatusOrders.AddAsync(newStatusOrder);
            await dbContext.SaveChangesAsync();

            return newStatusOrder;
        }

        public async Task<string?> DeleteStatusOrder(StatusOrder statusOrder)
        {
            var statusOrderExists = await dbContext.StatusOrders.FirstOrDefaultAsync(so=>so.StatusOrderId == statusOrder.StatusOrderId);

            if (statusOrderExists is null) return null;

            dbContext.Remove(statusOrderExists);
            await dbContext.SaveChangesAsync();

            return "Status Order has been deleted";
        }

        public async Task<List<StatusOrder>> GetAllStatusOrdersAsync()
        {
            return await dbContext.StatusOrders.ToListAsync();
        }

        public async Task<StatusOrder> GetStatusOrderAsync(StatusOrder statusOrder)
        {
            var statusOrderExists = await dbContext.StatusOrders.FirstOrDefaultAsync(so=>so.StatusDescrption == statusOrder.StatusDescrption && so.StatusOrderId==statusOrder.StatusOrderId);

            if (statusOrderExists is null) throw new Exception("Given Status Order does not exists");

            return statusOrderExists;
        }

        public async Task<StatusOrder> UpdateStatusOrderAsync(StatusOrder statusOrder, string userName)
        {
            var statusOrderExists = await dbContext.StatusOrders.FirstOrDefaultAsync(so => so.StatusOrderId == statusOrder.StatusOrderId);

            if (statusOrderExists is null) throw new Exception("Given Status Order could not be updated");

            statusOrderExists.StatusDescrption = statusOrder.StatusDescrption;
            await dbContext.SaveChangesAsync();

            return statusOrderExists;
        }
    }
}
