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
    public class GymLocationController : ControllerBase
    {
        private readonly IGymLocationService gymLocationService;

        public GymLocationController(IGymLocationService gymLocationService)
        {
            this.gymLocationService = gymLocationService;
        }

        [HttpGet("get-all-gym-locations")]
        public async Task<IActionResult> GetAllGymLocationsAsync()
        {
            try
            {
                return Ok(await gymLocationService.GetGymLocationsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-gym-location")]
        public async Task<IActionResult> GetGymLocationAsync([FromBody]GymLocation gymLocation)
        {
            try
            {
                return Ok(await gymLocationService.GetGymLocationAsync(gymLocation));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-gym-location")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddGymLocationAsync([FromBody]AddGymLocationRequest gymLocationRequest)
        {
            try
            {
                var result = await gymLocationService.AddGymLocationAsync(gymLocationRequest.GymLocation, gymLocationRequest.Username);

                if (result is null) return BadRequest("Could not add new gym location");

                return Created(nameof(AddGymLocationAsync), result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-gym-location")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateGymLocationAsync([FromBody]UpdateGymLocationRequest gymLocationRequest)
        {
            try
            {
                var result = await gymLocationService.UpdateGymLocationAsync(gymLocationRequest.GymLocation, gymLocationRequest.Username);

                if (result is null) return BadRequest("Could not update given gym location");

                return Ok (result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-gym-location")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteGymLocationAsync([FromBody]GymLocation gymLocation)
        {
            try
            {
                var result = await gymLocationService.DeleteGymLocation(gymLocation);

                if (result is null) return BadRequest("Could not delete given gym location");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
