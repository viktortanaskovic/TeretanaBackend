using Microsoft.AspNetCore.Identity;

namespace TeretanaBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Custom { get; set; } = string.Empty;
    }
}
