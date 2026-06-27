using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Dto
{
    public class CartOrderRequest
    {
        [Required]
        public int CartId { get; set; }
        [Required]
        public string User { get; set; } = null!;
        [Required]
        public string LocationSend { get; set; } = null!;
    }
}
