using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class AddGymLocationRequest
    {
        [Required]
        public GymLocation GymLocation { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;
    }
}
