using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeretanaBackend.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public bool OrderMade { get; set; } = false;
    }
}
