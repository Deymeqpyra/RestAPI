using Application.Courses.Exceptions;
using FluentValidation;

namespace Application.Courses.Commands;

public class FinishCourseForUsersCommandValidator : AbstractValidator<FinishCourseForUsersCommand>
{
    public FinishCourseForUsersCommandValidator()
    {
        RuleFor(c => c.CourseId).NotEmpty();
    }
}