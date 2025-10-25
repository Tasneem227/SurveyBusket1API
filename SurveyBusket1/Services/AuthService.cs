
using Microsoft.AspNetCore.Identity;
using SurveyBusket1.Abstractions;
using System.Security.Cryptography;

namespace SurveyBusket8.Services;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly IJwtProvider _JwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse?>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        //Check User?
        var user =await _UserManager.FindByEmailAsync(email);
        if (user == null) {
            return Result.Failure<AuthResponse?>(UserErrors.InvalidCredentials);
        }
        //Check Password
        var isPasswordValid =await _UserManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return Result.Failure<AuthResponse?>(UserErrors.InvalidCredentials);
        }
        //Generate JWT Token
        var (token, expiresIn) = _JwtProvider.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpirtion= DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken {Token= refreshToken, ExpiresOn= refreshTokenExpirtion });
        await _UserManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpirtion);
        return Result.Success(response);
    }

    public async Task<Result<AuthResponse?>> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId= _JwtProvider.ValidateToken(token);
        if (userId == null) return Result.Failure<AuthResponse?>(UserErrors.InvalidJwtToken);

        var user =await _UserManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure<AuthResponse?>(UserErrors.InvalidJwtToken);

        var existingRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == RefreshToken && rt.IsActive);
        if (existingRefreshToken == null)
            return Result.Failure<AuthResponse?>(UserErrors.InvalidRefreshToken);

        existingRefreshToken.RevokedOn = DateTime.UtcNow;
        //Generate new JWT Token & Refresh Token

        var (newtoken, expiresIn) = _JwtProvider.GenerateToken(user);
        var newrefreshToken = GenerateRefreshToken();
        var refreshTokenExpirtion = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken { Token = newtoken, ExpiresOn = refreshTokenExpirtion });
        await _UserManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newtoken, expiresIn, newrefreshToken, refreshTokenExpirtion);
        return Result.Success(response);


    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _JwtProvider.ValidateToken(token);
        if (userId == null)
           return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _UserManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var existingRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == RefreshToken && rt.IsActive);
        if (existingRefreshToken == null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        //Revoke  Refresh Token
        existingRefreshToken.RevokedOn = DateTime.UtcNow;
        await _UserManager.UpdateAsync(user);

        return Result.Success();
    }


    public static string GenerateRefreshToken()=> Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
