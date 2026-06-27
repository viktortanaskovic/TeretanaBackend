using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Dto;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class MembershipPlanService : IMembershipPlanService
    {
        private readonly AppDbContext dbContext;

        public MembershipPlanService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MembershipPlan> AddMembershipPlanAsync(MembershipPlan membershipPlan, string userName)
        {
            var planExists = await dbContext.MembershipPlans.FirstOrDefaultAsync(mp => mp.MembershipPlanDescription == membershipPlan.MembershipPlanDescription && mp.DurationInDays == membershipPlan.DurationInDays);

            if (planExists is not null) throw new Exception("Given Membership Plan already exists");

            var newPlan= new MembershipPlan()
            {
                MembershipPlanDescription = membershipPlan.MembershipPlanDescription,
                DurationInDays = membershipPlan.DurationInDays,
                Price = membershipPlan.Price,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName
            };

            await dbContext.MembershipPlans.AddAsync(newPlan);
            await dbContext.SaveChangesAsync();

            return newPlan;
        }

        public async Task<UserMembership> AddUserMemebershipAsync(UserMembershipRequest request)
        {
            var currentTime = DateTime.UtcNow;
            var membershipExists = await dbContext.UserMemberships.FirstOrDefaultAsync(um=>um.UserId== request.UserId && um.IsActive==true && um.StartDate<= currentTime && um.EndDate>= currentTime && um.MembershipPlanId==request.MembershipPlanId);

            if (membershipExists is not null) throw new Exception("User already has Membership Plan");

            var newUserMembership = new UserMembership() { 
                MembershipPlanId = request.MembershipPlanId,
                UserId = request.UserId,
                StartDate=request.StartTime,
                EndDate=request.EndTime,
                IsActive=true,
                CreatedAt=currentTime,
                CreatedBy=request.UserName
            };

            await dbContext.UserMemberships.AddAsync(newUserMembership);
            await dbContext.SaveChangesAsync();

            return newUserMembership;
        }

        public async Task<string?> DeleteMembershipPlan(MembershipPlan membershipPlan)
        {
            var planExists = await dbContext.MembershipPlans.FirstOrDefaultAsync(mp=>mp.MembershipPlanId == membershipPlan.MembershipPlanId);

            if (planExists is null) return null;

            dbContext.MembershipPlans.Remove(planExists);
            await dbContext.SaveChangesAsync();

            return "Membership Plan has been deleted";
        }

        public async Task<List<MembershipPlan>> GetAllMemberShipPlansAsync()
        {
            return await dbContext.MembershipPlans.ToListAsync();
        }

        public async Task<MembershipPlan> GetMembershipPlanAsync(MembershipPlan membershipPlan)
        {
            var planExists = await dbContext.MembershipPlans.FirstOrDefaultAsync(mp=>mp.MembershipPlanId==membershipPlan.MembershipPlanId && mp.MembershipPlanDescription==membershipPlan.MembershipPlanDescription);

            if (planExists is null) throw new Exception("Given Membership Plan does not exists");

            return planExists;
        }

        public async Task<MembershipPlan> UpdateMembershipPlanAsync(MembershipPlan membershipPlan, string userName)
        {
            var planExists = await dbContext.MembershipPlans.FirstOrDefaultAsync(mp=>mp.MembershipPlanId==membershipPlan.MembershipPlanId);

            if (planExists is null) throw new Exception("Given Membership Plan could not be updated and does not exists");

            planExists.MembershipPlanDescription=membershipPlan.MembershipPlanDescription;
            planExists.DurationInDays = membershipPlan.DurationInDays;
            planExists.Price = membershipPlan.Price;
            await dbContext.SaveChangesAsync();

            return planExists;
        }
    }
}
