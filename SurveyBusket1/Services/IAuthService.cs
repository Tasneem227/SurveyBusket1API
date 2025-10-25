
using SurveyBusket1.Abstractions;

namespace SurveyBusket8.Services;

public interface IAuthService
{
    Task<Result<AuthResponse?>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse?>> GetRefreshTokenAsync(string token,string RefreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token,string RefreshToken, CancellationToken cancellationToken = default);
}
