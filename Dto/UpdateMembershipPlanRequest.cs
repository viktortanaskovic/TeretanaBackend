using System.ComponentModel.DataAnnotations;
using TeretanaBackend.Models;

namespace TeretanaBackend.Dto
{
    public class UpdateMembershipPlanRequest
    {
        [Required]
        public MembershipPlan MembershipPlan { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
    }
}
