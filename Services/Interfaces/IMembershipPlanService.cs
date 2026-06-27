using TeretanaBackend.Dto;
using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface IMembershipPlanService
    {
        Task<MembershipPlan> AddMembershipPlanAsync(MembershipPlan membershipPlan, string userName);
        Task<List<MembershipPlan>> GetAllMemberShipPlansAsync();
        Task<MembershipPlan> GetMembershipPlanAsync(MembershipPlan membershipPlan);
        Task<MembershipPlan> UpdateMembershipPlanAsync(MembershipPlan membershipPlan, string userName);
        Task<string?> DeleteMembershipPlan(MembershipPlan membershipPlan);
        Task<UserMembership> AddUserMemebershipAsync(UserMembershipRequest request);
    }
}
