using TeretanaBackend.Models;

namespace TeretanaBackend.Services.Interfaces
{
    public interface IGymLocationService
    {
        Task<GymLocation> AddGymLocationAsync(GymLocation gymLocation, string userName);
        Task<List<GymLocation>> GetGymLocationsAsync();
        Task<GymLocation> GetGymLocationAsync(GymLocation gymLocation);
        Task<GymLocation> UpdateGymLocationAsync(GymLocation gymLocation, string userName);
        Task<string?> DeleteGymLocation(GymLocation gymLocation);
    }
}
