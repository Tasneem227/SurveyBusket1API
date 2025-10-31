namespace SurveyBusket1.Contracts.Questions;

public class QuestionValidator:AbstractValidator<QuestionRequest>
{
    public QuestionValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Question content is required.")
            .Length(3,1000).WithMessage("Question content must be between 3 and 1000 letters.");

        RuleFor(x => x.Answers).NotNull().WithMessage("Answers are required.");

        RuleFor(x => x.Answers)
            .Must(answers => answers.Count > 1)
            .WithMessage("At least two answers is required.")
            .When(X=>X.Answers != null);

        RuleFor(x => x.Answers)
            .Must(X=>X.Distinct().Count() == X.Count())
            .WithMessage("You Can't Add Duplicate Answers For the Same Question.")
            .When(X=>X.Answers != null);

        //.ForEach(answerRule =>
        //{
        //    answerRule
        //        .NotEmpty().WithMessage("Answer content is required.")
        //        .MaximumLength(1000).WithMessage("Answer content must not exceed 1000 characters.");
        //});
    }
}
