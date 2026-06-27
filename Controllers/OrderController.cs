using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using TeretanaBackend.Dto;
using TeretanaBackend.Services.Interfaces;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService= orderService;
        }
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrderAsync(OrderRequest request)
        {
            try
            {
                var result = await orderService.AddOrderAsync(request);
                if (result is null) return BadRequest("Could not create new Order");
                return Created(nameof(AddOrderAsync), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-order-with-items")]
        public async Task<IActionResult> AddOrderWithItems(OrderWithItemsRequest request)
        {
            try
            {
                var result = await orderService.AddOrderWithItemsAsync(request);
                if (result is null) return BadRequest("Could not create Order with OrderItems");
                return Created(nameof(AddOrderWithItems), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-order")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteOrder(OrderRequest request)
        {
            try
            {
                var result = await orderService.DeleteOrder(request);
                if (result is null) return BadRequest("Could not delete Order");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-order")]
        public async Task<IActionResult> GetOrder(OrderRequest orderRequest)
        {
            try
            {
                var result = await orderService.GetOrderAsync(orderRequest);
                if (result is null) return BadRequest("Could not get Order from db");
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-orders")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var result = await orderService.GetOrdersAsync();
                if (result is null) return BadRequest("Could not get all Orders");
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-order-with-items")]
        public async Task<IActionResult> GetOrderWithItems(OrderRequest request)
        {
            try
            {
                var result = await orderService.GetOrderWithItemsAsync(request);
                if (result is null) return BadRequest("Could not get Order with OrderItems");
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-payment-order")]
        public async Task<IActionResult> OrderPayment(OrderPaymentRequest request)
        {
            try
            {
                var result = await orderService.OrderPaymentAsync(request);
                if (result is null) return BadRequest("Could not make Payment for Order");
                return Created(nameof(OrderPayment), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-order")]
        public async Task<IActionResult> UpdateOrder(OrderRequest request)
        {
            try
            {
                var result = await orderService.UpdateOrderAsync(request);
                if (result is null) return BadRequest("Could not update given Order");
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-order-with-order-items")]
        public async Task<IActionResult> UpdateOrderWithOrderItems(OrderWithItemsRequest request)
        {
            try
            {
                var result = await orderService.UpdateOrderWithItemsAsync(request);
                if (result is null) return BadRequest("Could not update Order with OrderItems");
                return Created(nameof(UpdateOrderWithOrderItems), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-order-with-items-from-cartid")]
        public async Task<IActionResult> AddOrderWithItemsFromCartId(CartOrderRequest request)
        {
            try
            {
                var result = await orderService.AddOrderWithItemsFromCartIdAsync(request);
                if (result is null) return BadRequest("Could not add Order with Items from CartId");
                return Created(nameof(AddOrderWithItemsFromCartId), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
