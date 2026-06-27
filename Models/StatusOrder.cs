using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Models
{
    public class StatusOrder
    {
        public int StatusOrderId { get; set; }

        [Required]
        public string StatusDescrption { get; set; } = null!;

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
