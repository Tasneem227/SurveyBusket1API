

namespace SurveyBusket1.Errors;

public static class UserErrors
{
    public static Error InvalidCredentials = 
        new("User.InvalidCredentials", "Invalid Email/Password");

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token");

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token");
}
