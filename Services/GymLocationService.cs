using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Data;
using TeretanaBackend.Models;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class GymLocationService : IGymLocationService
    {
        private readonly AppDbContext dbContext;

        public GymLocationService(AppDbContext dbContext)
        {
            this.dbContext= dbContext;
        }

        public async Task<GymLocation> AddGymLocationAsync(GymLocation gymLocation, string userName)
        {
            var locationExists = await dbContext.GymLocations.FirstOrDefaultAsync(gl => gl.CityName == gymLocation.CityName && gl.Street==gymLocation.Street && gl.ZipCode==gymLocation.ZipCode);

            if (locationExists is not null) throw new Exception("Gym location already exists");

            var newGymLocation = new GymLocation()
            {
                CityName = gymLocation.CityName,
                Street = gymLocation.Street,
                ZipCode = gymLocation.ZipCode,
                CreatedAt = DateTime.Now,
                CreatedBy = userName
            };

            await dbContext.GymLocations.AddAsync(newGymLocation);
            await dbContext.SaveChangesAsync();

            return newGymLocation;
        }

        public async Task<string?> DeleteGymLocation(GymLocation gymLocation)
        {
            var locationExists = await dbContext.GymLocations.FirstOrDefaultAsync(gl => gl.GymLocationId == gymLocation.GymLocationId);

            if (locationExists is null) return null;

            dbContext.GymLocations.Remove(locationExists);
            await dbContext.SaveChangesAsync();

            return "Gym location deleted successfully";
        }

        public async Task<GymLocation> GetGymLocationAsync(GymLocation gymLocation)
        {
            var locationExists = await dbContext.GymLocations.FirstOrDefaultAsync(gl=>gl.GymLocationId==gymLocation.GymLocationId);

            if (locationExists is null) throw new Exception("Gym location does not exists");

            return locationExists;
        }

        public async Task<List<GymLocation>> GetGymLocationsAsync()
        {
            return await dbContext.GymLocations.ToListAsync();
        }

        public async Task<GymLocation> UpdateGymLocationAsync(GymLocation gymLocation, string userName)
        {
            var locationExists = await dbContext.GymLocations.FirstOrDefaultAsync(gl => gl.GymLocationId == gymLocation.GymLocationId);

            if (locationExists is null) throw new Exception("Gym location does not exists");

            locationExists.Street = gymLocation.Street;
            locationExists.CityName=gymLocation.CityName;
            locationExists.ZipCode=gymLocation.ZipCode;

            await dbContext.SaveChangesAsync();

            return locationExists;
        }
    }
}
