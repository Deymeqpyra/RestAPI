using FluentValidation;

namespace Application.Courses.Commands;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x=>x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x=>x.MaxStudents).NotEmpty().GreaterThan(0).LessThan(200);
        RuleFor(x=>x.StartDate)
            .NotEmpty()
            .LessThan(x=>x.EndDate)
            .GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(x=>x.EndDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
    }
}