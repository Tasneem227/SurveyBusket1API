namespace SurveyBusket1.Contracts.Authentication;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
    );

