using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Dto
{
    public class OrderRequest
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string LocationSend { get; set; } = null!;
    }
}
