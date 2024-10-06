using FluentValidation;

namespace Application.Courses.Commands;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(v => v.CourseId).NotEmpty();
        RuleFor(v=>v.Name).NotEmpty().MaximumLength(255).MinimumLength(2);
        RuleFor(v => v.StartDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(v => v.EndDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(v=>v.MaxStudents).NotEmpty().GreaterThan(0).LessThanOrEqualTo(200);
    }
}