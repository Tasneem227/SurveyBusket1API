namespace SurveyBusket8.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequestUser>
{
    public LoginRequestValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty();

    }
}