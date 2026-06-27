using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class OrderWithItemsResponse
    {
        [Required]
        public Order Order { get; set; } = null!;
        [Required]
        public List<OrderItem> OrderItems { get; set; } = null!;
    }
}
