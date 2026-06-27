using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class OrderWithItemsRequest
    {
        [Required]
        public OrderRequest Order { get; set; } = null!;
        [Required]
        public List<OrderItem> OrderItems { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
    }
}
