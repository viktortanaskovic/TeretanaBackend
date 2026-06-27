namespace TeretanaBackend.Models
{
    public class GymLocation
    {
        public int GymLocationId { get; set; }

        public string CityName { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
