using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Dto;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext dbContext;

        public ProductService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> AddProductAsync(ProductRequest request)
        {
            var productExists = await dbContext.Products.FirstOrDefaultAsync(p=>p.ProductName == request.ProductName && p.Price==request.Price);

            if (productExists is not null) throw new Exception("Given Product already exists");

            var newProduct = new Product() { 
                CategoryId= request.CategoryId,
                Price= request.Price,
                ProductName= request.ProductName,
                ProductDescription= request.ProductDescription,
                StockQuantity= request.StockQuantity,
                CreatedAt= DateTime.UtcNow,
                CreatedBy= request.UserName
            };
            if(request.ImageUrl is not null) newProduct.ImageUrl = request.ImageUrl;
            
            await dbContext.Products.AddAsync(newProduct);
            await dbContext.SaveChangesAsync();

            return newProduct;
        }

        public async Task<string?> DeleteProduct(ProductRequest request)
        {
            var productExists = await dbContext.Products.FirstOrDefaultAsync(p=>p.ProductName==request.ProductName && p.Price==request.Price);

            if (productExists is null) throw new Exception("Given Product does not exists");

            dbContext.Remove(productExists);
            await dbContext.SaveChangesAsync();

            return "Given Product has been deleted";
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductAsync(ProductRequest request)
        {
            var productExists = await dbContext.Products.FirstOrDefaultAsync(p=>p.ProductName== request.ProductName && p.Price == request.Price);

            if (productExists is null) throw new Exception("Given Product does not exists");

            return productExists;
        }

        public async Task<Product> UpdateProductAsync(ProductRequest request)
        {
            var productExists = await dbContext.Products.FirstOrDefaultAsync(p=>p.ProductId== request.ProductId);

            if (productExists is null) throw new Exception("Given Product does not exists");

            productExists.ProductName = request.ProductName;
            productExists.ProductDescription = request.ProductDescription;
            productExists.Price = request.Price;
            productExists.StockQuantity = request.StockQuantity;
            productExists.CategoryId = request.CategoryId;
            productExists.ModifiedAt = DateTime.UtcNow;
            productExists.ModifiedBy = request.UserName;
            if(request.ImageUrl is not null) productExists.ImageUrl = request.ImageUrl;

            await dbContext.SaveChangesAsync();

            return productExists;
        }
    }
}
