using FluentValidation;

namespace SurveyBusket8.Validations;

//public class StudentValidator:AbstractValidator<Student>
//{
//    public StudentValidator()
//    {
//        RuleFor(x=>x.dateofbirth)
//            .Must(BeMoreThan18Years)
//            .When(x=> x.dateofbirth.HasValue)
//            .WithMessage("{PropertyName} is invalid dob should be at least 18 ");

       
//    }
//    private bool BeMoreThan18Years(DateTime? date)
//    {
        
//            return DateTime.Today > date.Value.AddYears(18);
        
//    }
//}
