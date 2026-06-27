using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class AddCartRequest
    {
        [Required]
        public Cart Cart { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
    }
}
