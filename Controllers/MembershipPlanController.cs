using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeretanaBackend.Dto;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipPlanService membershipPlanService;

        public MembershipPlanController(IMembershipPlanService membershipPlanService)
        {
            this.membershipPlanService = membershipPlanService;
        }

        [HttpGet("get-membership-plans")]
        public async Task<IActionResult> GetAllMembershiplPlans()
        {
            try
            {
                return Ok(await membershipPlanService.GetAllMemberShipPlansAsync());
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-membership")]
        public async Task<IActionResult> GetMembershipPlan(MembershipPlan membershipPlan)
        {
            try
            {
                return Ok(await membershipPlanService.GetMembershipPlanAsync(membershipPlan));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-membership-plan")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddMembershipPlan(AddMembershipPlanRequest request)
        {
            try
            {
                var result = await membershipPlanService.AddMembershipPlanAsync(request.MembershipPlan, request.UserName);

                if (result is null) return BadRequest("Could not create Memebership Plan");

                return Created(nameof(AddMembershipPlan), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-membership-plan")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateMembershipPlan(UpdateMembershipPlanRequest request)
        {
            try
            {
                var result = await membershipPlanService.UpdateMembershipPlanAsync(request.MembershipPlan, request.UserName);

                if (result is null) return BadRequest("Could not update Membership Plan");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-membership-plan")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteMembershipPlan(MembershipPlan membershipPlan)
        {
            try
            {
                var result = await membershipPlanService.DeleteMembershipPlan(membershipPlan);

                if (result is null) return BadRequest("Could not delete Membership Plan");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-user-membership")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddUserMembership([FromBody]UserMembershipRequest request)
        {
            try
            {
                var result = await membershipPlanService.AddUserMemebershipAsync(request);

                if (result is null) return BadRequest("Could not add User Membership Plan");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
