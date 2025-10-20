
namespace SurveyBusket8.Services;

public interface IAuthService
{
    Task<AuthResponse> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetRefreshTokenAsync(string token,string RefreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshTokenAsync(string token,string RefreshToken, CancellationToken cancellationToken = default);
}
