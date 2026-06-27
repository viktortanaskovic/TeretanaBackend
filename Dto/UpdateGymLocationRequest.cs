using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class UpdateGymLocationRequest
    {
        [Required]
        public GymLocation GymLocation { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;
    }
}
