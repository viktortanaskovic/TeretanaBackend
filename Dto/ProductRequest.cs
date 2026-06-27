
using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Dto
{
    public class ProductRequest
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
    }
}
