using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeretanaBackend.Dto;
using TeretanaBackend.Services.Interfaces;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("add-product")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddProduct(ProductRequest request)
        {
            try
            {
                var result = await productService.AddProductAsync(request);

                if (result is null) return BadRequest("Could not add Product");

                return Created(nameof(AddProduct), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                return Ok(await productService.GetAllProductsAsync());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct([FromBody]ProductRequest request)
        {
            try
            {
                var result = await productService.GetProductAsync(request);

                if (result is null) return NotFound();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-product")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateProduct([FromBody]ProductRequest request)
        {
            try
            {
                var result = await productService.UpdateProductAsync(request);

                if (result is null) return BadRequest("could not update Product");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-product")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteProduct([FromBody]ProductRequest request)
        {
            try
            {
                var result = await productService.DeleteProduct(request);

                if (result is null) return BadRequest();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
