using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeretanaBackend.Models
{
    public class UserMembership
    {
        public int UserMembershipId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int MembershipPlanId { get; set; }
        [ForeignKey(nameof(MembershipPlanId))]
        public MembershipPlan MembershipPlan { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
