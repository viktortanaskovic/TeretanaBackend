using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Dto;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext dbContext;

        public OrderService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order> AddOrderAsync(OrderRequest request)
        {
            var unpayedOrderExists = await dbContext.Orders.FirstOrDefaultAsync(o=>o.UserId == request.UserId && o.StatusOrder.StatusDescrption.Equals(StatusOfOrders.UnpayedOrder));
            if (unpayedOrderExists is not null) throw new Exception("User already has an Order which is not payed");

            var newOrder = new Order()
            {
                UserId=request.UserId,
                StatusId=request.StatusId,
                TotalPrice=request.TotalPrice,
                CreatedBy=request.UserName,
                CreatedAt=DateTime.UtcNow,
                LocationSend=request.LocationSend
            };
            await dbContext.Orders.AddAsync(newOrder);
            await dbContext.SaveChangesAsync();

            return newOrder;
        }

        public async Task<OrderWithItemsResponse> AddOrderWithItemsAsync(OrderWithItemsRequest request)
        {
            var unpayedOrderExists = await dbContext.Orders.FirstOrDefaultAsync(o => o.UserId == request.Order.UserId && o.StatusOrder.StatusDescrption.Equals(StatusOfOrders.UnpayedOrder));
            if (unpayedOrderExists is not null) throw new Exception("User already has an Order which is not payed");

            var newOrder = new Order()
            {
                StatusId=request.Order.StatusId,
                UserId=request.Order.UserId,
                TotalPrice=request.Order.TotalPrice,
                CreatedAt = DateTime.UtcNow,
                CreatedBy=request.UserName,
                LocationSend=request.Order.LocationSend
            };
            await dbContext.Orders.AddAsync(newOrder);
            await dbContext.SaveChangesAsync();


            var orderItemsList = request.OrderItems;
            var newOrderItemsList = new List<OrderItem>();

            foreach (var item in orderItemsList)
            {
                if (!await dbContext.Products.Where(p => p.ProductId == item.ProductId && p.StockQuantity >= item.Quantity).AnyAsync()) throw new Exception("Product does not exists or there is no enough in Stock Quantity");
                var newOrderItem = new OrderItem()
                {
                    ProductId=item.ProductId,
                    Quantity=item.Quantity,
                    Price=item.Price,
                    OrderId=newOrder.OrderId,
                    CreatedAt=DateTime.UtcNow,
                    CreatedBy=request.UserName
                };
                newOrderItemsList.Add(newOrderItem);
            }
            if(newOrderItemsList.Count>0)
            {
                await dbContext.OrderItems.AddRangeAsync(newOrderItemsList);
                await dbContext.SaveChangesAsync();
            }
            var result = new OrderWithItemsResponse() { Order =  newOrder, OrderItems = newOrderItemsList};
            return result;
        }

        public async Task<string?> DeleteOrder(OrderRequest request)
        {
            var orderExists = await dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == request.OrderId);
            if (orderExists is null) throw new Exception("Given Order does not exists");
            dbContext.Remove(orderExists);
            await dbContext.SaveChangesAsync();
            return "Order has been deleted";
        }

        public async Task<Order> GetOrderAsync(OrderRequest request)
        {
            var orderExists = await dbContext.Orders.FirstOrDefaultAsync(o=>o.OrderId== request.OrderId);
            if (orderExists is null) throw new Exception("Given Order does not exists");
            return orderExists;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await dbContext.Orders.ToListAsync();
        }

        public async Task<OrderWithItemsResponse> GetOrderWithItemsAsync(OrderRequest request)
        {
            var response = new OrderWithItemsResponse();

            var orderExists = await dbContext.Orders.FirstOrDefaultAsync(o => o.UserId == request.UserId && o.TotalPrice == request.TotalPrice && o.StatusId == request.StatusId);

            if (orderExists is null) throw new Exception("There is no given Order");

            response.Order = orderExists;

            var orderItems = await dbContext.OrderItems.Where(oi=>oi.OrderId == orderExists.OrderId).ToListAsync();
            if (orderItems is null) throw new Exception("This Order has no Items");

            response.OrderItems = orderItems;

            return response;
        }

        public async Task<OrderPaymentResponse> OrderPaymentAsync(OrderPaymentRequest request)
        {
            var response = new OrderPaymentResponse();

            var orderExists = await dbContext.Orders.FirstOrDefaultAsync(o=>o.OrderId == request.OrderId);
            var statusOfOrders = await dbContext.StatusOrders.ToListAsync();
            if (orderExists is null) throw new Exception("Given Order does not exists");

            var payedOrder = await dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == orderExists.OrderId);
            if (payedOrder is not null && payedOrder.Amount == orderExists.TotalPrice) throw new Exception("You have Payed for given Order");
            else if(payedOrder is null && request.Amount == orderExists.TotalPrice) 
            {
                var newPayment = new Payment() { OrderId = orderExists.OrderId, Amount = request.Amount, CreatedAt = DateTime.UtcNow, CreatedBy = request.UserName };
                await dbContext.Payments.AddAsync(newPayment);
                orderExists.StatusId = statusOfOrders.Where(so => so.StatusDescrption.Equals(StatusOfOrders.PayedOrder)).FirstOrDefault().StatusOrderId;
                await dbContext.SaveChangesAsync();

                response.Order = orderExists;
                response.Payment = newPayment;
            }
            return response;
        }

        public async Task<Order> UpdateOrderAsync(OrderRequest request)
        {
            var orderExists = await dbContext.Orders.FirstOrDefaultAsync(o=>o.OrderId==request.OrderId);
            if (orderExists is null) throw new Exception("There is no given Order");

            orderExists.StatusId= request.StatusId;
            orderExists.UserId = request.UserId;
            orderExists.TotalPrice = request.TotalPrice;
            await dbContext.SaveChangesAsync();
            return orderExists;
        }

        public async Task<OrderWithItemsResponse> UpdateOrderWithItemsAsync(OrderWithItemsRequest request)
        {
            var order = request.Order;
            var orderItems = request.OrderItems;

            var orderExists = await dbContext.Orders.FirstOrDefaultAsync(o=>o.OrderId==order.OrderId);

            if (orderExists is null) throw new Exception("Given Order does not exists");

            orderExists.StatusId= order.StatusId;
            orderExists.UserId= order.UserId;
            orderExists.TotalPrice= order.TotalPrice;

            var itemsForGivenOrder = await dbContext.OrderItems.Where(i => i.OrderId == orderExists.OrderId).ToListAsync();

            dbContext.OrderItems.RemoveRange(itemsForGivenOrder);
            await dbContext.AddRangeAsync(orderItems);
            await dbContext.SaveChangesAsync();
            var response = new OrderWithItemsResponse();
            response.Order = orderExists;
            response.OrderItems = orderItems;

            return response;
        }

        public async Task<OrderWithItemsResponse> AddOrderWithItemsFromCartIdAsync(CartOrderRequest request)
        {
            int cartId = request.CartId;
            string user = request.User;
            string locationSend = request.LocationSend;
            if (string.IsNullOrEmpty(user) || cartId <= 0 || string.IsNullOrEmpty(locationSend)) throw new Exception("Given request parameters have invalid values");

            var cart = await dbContext.Carts.FirstOrDefaultAsync(c=>c.CartId==cartId);
            if (cart is null) throw new Exception("Cart does not exists");
            if (cart is not null && cart.OrderMade) throw new Exception("Order has been made for this Cart");

            var cartItems = await dbContext.CartItems.Where(ci => ci.CartId == cart.CartId).ToListAsync();
            if (cartItems.Count == 0) throw new Exception("CartItems list is empty");
            var userDb = await dbContext.Users.FirstOrDefaultAsync(u=>u.UserName==user);
            if (userDb is null) throw new Exception("There is no User with given UserName");

            List<OrderItem> orderItems = new List<OrderItem>();
            OrderRequest order = new OrderRequest();
            var statusUnpayed = await dbContext.StatusOrders.FirstOrDefaultAsync(so => so.StatusDescrption == StatusOfOrders.UnpayedOrder);
            if (statusUnpayed is null) throw new Exception("Status Unpaid does not exist in DB");
            order.StatusId = statusUnpayed.StatusOrderId;
            order.UserId = userDb.Id;
            order.UserName= user;
            order.LocationSend= locationSend;
            OrderItem orderItem = new OrderItem();
            var listProducts = await dbContext.Products.ToListAsync();
            if (listProducts.Count == 0) throw new Exception("There is no Products in DB");
            var totalPrice = 0.0;
            foreach (var cartItem in cartItems)
            {
                orderItem = new OrderItem();
                orderItem.ProductId = cartItem.ProductId;
                orderItem.Quantity = cartItem.Quantity;
                var priceProduct = listProducts.FirstOrDefault(p=>p.ProductId == cartItem.ProductId).Price;
                if (priceProduct <= 0) throw new Exception("Invalid Product");
                var totalPriceProduct = priceProduct * cartItem.Quantity;
                orderItem.Price= totalPriceProduct;
                orderItem.CreatedAt = DateTime.UtcNow;
                orderItem.CreatedBy = user;
                orderItems.Add(orderItem);
                totalPrice += totalPriceProduct;
            }
            order.TotalPrice= totalPrice;
            OrderWithItemsRequest newRequest = new OrderWithItemsRequest();
            newRequest.Order = order;
            newRequest.OrderItems = orderItems;
            newRequest.UserName = user;
            var response =  await AddOrderWithItemsAsync(newRequest);
            if (response is not null)
            {
                cart.OrderMade = true;
                await dbContext.SaveChangesAsync();
                return response;
            }
            else throw new Exception("There is exception for making Order with CartItems and Cart which is given in request");
        }
    }
}
