using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> AddCategoryAsync(Category category, string userName)
        {
            var categoryExists = await dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == category.CategoryName);

            if (categoryExists is not null) throw new Exception("Given category already exists");

            var newCategory = new Category()
            {
                CategoryName= category.CategoryName,
                CategoryDescription=category.CategoryDescription,
                CreatedBy = userName,
                CreatedAt = DateTime.Now
            };

            await dbContext.Categories.AddAsync(newCategory);
            await dbContext.SaveChangesAsync();

            return newCategory;
        }

        public async Task<string?> DeleteCategory(Category category)
        {
            var categoryExists = await dbContext.Categories.FirstOrDefaultAsync(c=>c.CategoryName == category.CategoryName);

            if (categoryExists is null) return null;

            dbContext.Categories.Remove(categoryExists);
            await dbContext.SaveChangesAsync();

            return "Category has been deleted";
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(Category category)
        {
            var categoryExists = await dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if (categoryExists is null) throw new Exception("Category does not exists");

            return categoryExists;
        }

        public async Task<Category> UpdateCategoryAsync(Category category, string userName)
        {
            var categoryExists = await dbContext.Categories.FirstOrDefaultAsync(c=>c.CategoryId==category.CategoryId);

            if (categoryExists is null) throw new Exception("Category does not exists");

            categoryExists.CategoryName = category.CategoryName;
            categoryExists.CategoryDescription = category.CategoryDescription;
            categoryExists.ModifiedAt = DateTime.Now;
            categoryExists.ModifiedBy = userName;
            await dbContext.SaveChangesAsync();

            return categoryExists;
        }
    }
}
