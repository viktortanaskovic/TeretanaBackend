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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService= categoryService;
        }
       
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await categoryService.GetAllCategoriesAsync());
        }

        [HttpGet("get-category-by-id")]
        public async Task<IActionResult> GetCategoryById([FromBody]Category category)
        {
            try
            {
                return Ok(await categoryService.GetCategoryAsync(category));
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody]AddCategoryRequest addCategoryRequest)
        {
            try
            {
                var result = await categoryService.AddCategoryAsync(addCategoryRequest.Category, addCategoryRequest.UserName);

                if (result is null) return BadRequest("Category could not be created");

                return Created(nameof(AddCategory), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryRequest updateCategoryRequest)
        {
            try
            {
                var result = await categoryService.UpdateCategoryAsync(updateCategoryRequest.Category, updateCategoryRequest.UserName);

                if (result is null) return BadRequest("Category could not be updated");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete-category")]
        public async Task<IActionResult> DeleteCategory([FromBody]Category category)
        {
            try
            {
                var result = await categoryService.DeleteCategory(category);

                if (result is null) return BadRequest("Category could not be deleted");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
