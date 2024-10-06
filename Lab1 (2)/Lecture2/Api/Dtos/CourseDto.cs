using Domain.Courses;

namespace Api.Dtos;

public record 
    CourseDto(
    Guid? CourseId,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int MaxStudents)
{
    public static CourseDto FromDomainModel(Course course)
        => new(
            CourseId: course.Id.Value,
            Name: course.Name,
            StartDate: course.StartDate,
            EndDate: course.EndDate,
            MaxStudents: course.MaxStudents
            );
}