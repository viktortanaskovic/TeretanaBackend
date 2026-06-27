using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeretanaBackend.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        public StatusOrder StatusOrder { get; set; } = null!;

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public string LocationSend { get; set; } = null!;
    }
}
