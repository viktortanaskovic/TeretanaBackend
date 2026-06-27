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
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("get-all-carts")]
        public async Task<IActionResult> GetAllCarts()
        {
            return Ok(cartService.GetAllCartsAsync());
        }

        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCart(Cart cart)
        {
            try
            {
                return Ok(await cartService.GetCartAsync(cart));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-cart")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddCart(AddCartRequest request)
        {
            try
            {
                var result = await cartService.AddCartAsync(request.Cart, request.UserName);

                if (result is null) return BadRequest("Could not create new Cart");

                return Created(nameof(AddCart), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-cart")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateCart(UpdateCartRequest request)
        {
            try
            {
                var result = await cartService.UpdateCartAsync(request.Cart, request.UserName);

                if (result is null) return BadRequest("Could not update given Cart");
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-cart")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteCart(Cart cart)
        {
            try
            {
                var result = await cartService.DeleteCart(cart);

                if (result is null) return BadRequest("Could not delete given Cart");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-cart-with-items")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetCartWithItems(Cart cart)
        {
            try
            {
                var result = await cartService.GetCartWithItemsAsync(cart);

                if (result is null) return BadRequest("Cart Items could not retrive");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-cart-with-items")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddCartWithItems(CartItemsRequest cartItemsRequest)
        {
            try
            {
                var result = await cartService.AddCartWIthItemsAsync(cartItemsRequest);

                if (result is null) return BadRequest("Could not add Cart with Cart Items");

                return Created(nameof(AddCartWithItems),result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-cart-with-items")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateCartWithItems(CartItemsRequest cartItemsRequest)
        {
            try
            {
                var result = await cartService.UpdateCartWithItemsAsync(cartItemsRequest);

                if (result is null) return BadRequest("Could not update Cart with given Items");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
