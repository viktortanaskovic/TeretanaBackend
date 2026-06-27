using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class UpdateCategoryRequest
    {
        [Required]
        public Category Category { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;
    }
}
