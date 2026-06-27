using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> AddCategoryAsync(Category category, string userName);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category, string userName);
        Task<string?> DeleteCategory(Category category);
    }
}
