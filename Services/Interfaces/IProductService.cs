using TeretanaBackend.Dto;
using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(ProductRequest request);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(ProductRequest request);
        Task<Product> UpdateProductAsync(ProductRequest request);
        Task<string?> DeleteProduct(ProductRequest request);
    }
}
