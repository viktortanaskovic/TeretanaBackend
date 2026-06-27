using System.ComponentModel.DataAnnotations;

namespace TeretanaBackend.Dto
{
    public class UserMembershipRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public int MembershipPlanId { get; set; }
        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;
        [Required]
        public DateTime EndTime { get; set; } = DateTime.Now;
        [Required]
        public string UserName { get; set; } = null!;
    }
}
