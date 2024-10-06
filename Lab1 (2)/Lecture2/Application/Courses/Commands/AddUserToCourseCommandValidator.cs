using FluentValidation;

namespace Application.Courses.Commands;

public class AddUserToCourseCommandValidator : AbstractValidator<AddUserToCourseCommand>
{
    public AddUserToCourseCommandValidator()
    {
        RuleFor(command => command.CourseId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();
    }
}