using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterVM payload);
        Task<AuthResultVM> LoginAsync(LoginVM payload);
        Task<AuthResultVM> RefreshTokenAsync(TokenRequestVM payload);
    }
}
