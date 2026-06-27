using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class OrderPaymentResponse
    {
        [Required]
        public Order Order { get; set; } = null!;

        [Required]
        public Payment Payment { get; set; } = null!;
    }
}
