using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x=>x.MaxStudents).GreaterThan(0).NotEmpty();
    }
}