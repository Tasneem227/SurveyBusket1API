namespace SurveyBusket1.Athentication;

public interface IJwtProvider
{
    public string? ValidateToken(string token); 
    (string token,int expiresIn) GenerateToken(ApplicationUser user);
}
