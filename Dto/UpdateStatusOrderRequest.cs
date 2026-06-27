using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class UpdateStatusOrderRequest
    {
        [Required]
        public StatusOrder StatusOrder { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
    }
}
