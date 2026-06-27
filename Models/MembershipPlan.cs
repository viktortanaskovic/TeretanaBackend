using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Models
{
    public class MembershipPlan
    {
        public int MembershipPlanId { get; set; }

        [Required]
        public string MembershipPlanDescription { get; set; } = null!;

        [Required]
        public double Price { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
