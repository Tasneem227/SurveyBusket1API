namespace SurveyBusket1.Contracts.Authentication;

public class RefreshTokenValidation : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidation()
    {

        RuleFor(x => x.Token)
            .NotEmpty();
        RuleFor(x => x.RefreshToken)
            .NotEmpty();

    }
}
