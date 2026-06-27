using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Dto
{
    public class OrderPaymentRequest
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string UserName { get; set; } = null!;
    }
}
