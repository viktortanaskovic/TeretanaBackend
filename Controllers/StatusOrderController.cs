using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeretanaBackend.Dto;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusOrderController : ControllerBase
    {
        private readonly IStatusOrderService statusOrderService;

        public StatusOrderController(IStatusOrderService statusOrderService)
        {
            this.statusOrderService = statusOrderService;
        }

        [HttpGet("get-all-status-orders")]
        public async Task<IActionResult> GetAllStatusOrders()
        {
            try
            {
                return Ok(await statusOrderService.GetAllStatusOrdersAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-status-order")]
        public async Task<IActionResult> GetStatusOrder(StatusOrder statusOrder)
        {
            try
            {
                return Ok(await statusOrderService.GetStatusOrderAsync(statusOrder));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-status-order")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddStatusOrder(AddStatusOrderRequest request)
        {
            try
            {
                var result = await statusOrderService.AddStatusOrderAsync(request.StatusOrder, request.UserName);

                if (result is null) return BadRequest("Could not add new Status Order");

                return Created(nameof(AddStatusOrder), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-status-order")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateStatusOrder(UpdateStatusOrderRequest request)
        {
            try
            {
                var result = await statusOrderService.UpdateStatusOrderAsync(request.StatusOrder, request.UserName);

                if (result is null) return BadRequest("Could not update given Status Order");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-status-order")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteStatusOrder(StatusOrder statusOrder)
        {
            try
            {
                var result = await statusOrderService.DeleteStatusOrder(statusOrder);

                if (result is null) return BadRequest("Could not delete given Status Order");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
