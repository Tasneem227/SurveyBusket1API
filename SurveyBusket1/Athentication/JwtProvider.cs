
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBusket1.Athentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider

{
    private readonly JwtOptions _Options = options.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //unique identifier for the token
        ];
        var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.key));
        var SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256); // algorithm to sign the token to ensure its integrity
        var expiresIn = _Options.ExpiryMinutes;
        var token = new JwtSecurityToken(
            issuer: _Options.Issuer,
            audience: _Options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: SigningCredentials
        );
        return (token: new JwtSecurityTokenHandler().WriteToken(token),// transform token object to string
            expiresIn: expiresIn * 60);
    }

    public string? ValidateToken(string token)
    {
        var TokenHandler = new JwtSecurityTokenHandler();
        var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.key));
        try
        {
            //encoding
            TokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = SymmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            } , out SecurityToken ValidatedToken);
            var JwtToken = (JwtSecurityToken) ValidatedToken;
            return JwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }


}

