using FluentValidation;

namespace SurveyBusket8.Contracts.Polls;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
    public PollRequestValidator() {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("add title please")
            .Length(3,100).WithMessage("TITLE SHOULD BE AT LEAST {MinLength} , AT MOST {MaxLength},  you entered {PropertyName}");
        RuleFor(x => x.Summary)
            .NotEmpty()
            .WithMessage("add summary please")
            .Length(3, 1500)
            .WithMessage("SUMMARY SHOULD BE AT LEAST {MinLength} , AT MOST {MaxLength},  you entered {PropertyName}"); 
        RuleFor(x=>x.StartsAt)
            .NotEmpty()
            .WithMessage("add start date please")
            .GreaterThan(DateTime.Now) // Ensure that the start date is greater than now
            .WithMessage("start date should be greater than now");
        RuleFor(x => x.EndsAt)
            .NotEmpty()
            .WithMessage("add End date please");
        RuleFor(x => x)
            .Must(HasValidDateRange)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} should be greater than start date");






    }
    private bool HasValidDateRange(PollRequest request)
    {
        return request.EndsAt > request.StartsAt;
    }
}
