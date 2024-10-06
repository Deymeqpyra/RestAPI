using FluentValidation;

namespace Application.Courses.Commands;

public class SetRatingCourseCommandValidator : AbstractValidator<SetRatingCourseCommand>
{
    public SetRatingCourseCommandValidator()
    {
        RuleFor(v=>v.Rating).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100);
    }
}