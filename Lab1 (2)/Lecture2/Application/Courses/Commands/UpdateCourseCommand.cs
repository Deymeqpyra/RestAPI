using System.Globalization;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using Domain.Faculties;
using MediatR;
using Optional;

namespace Application.Courses.Commands;

public class UpdateCourseCommand : IRequest<Result<Course, CourseException>>
{
    public required Guid CourseId { get; init; }
    public required string Name { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required int MaxStudents { get; init; }
}

public class UpdateCourseCommandHandler(ICourseRepository courseRepository)
    : IRequestHandler<UpdateCourseCommand, Result<Course, CourseException>>
{
    public async Task<Result<Course, CourseException>> Handle(
        UpdateCourseCommand request,
        CancellationToken cancellationToken
    )
    {
        var courseId = new CourseId(request.CourseId);
        var course = await courseRepository.GetById(courseId, cancellationToken);

        return await course.Match(
            async c => await UpdateEntity(c, request.Name, request.StartDate, request.EndDate,
                request.MaxStudents, cancellationToken),
            () => Task.FromResult<Result<Course, CourseException>>(new CourseNotFoundException(courseId))
         );

    }

    private async Task<Result<Course, CourseException>> UpdateEntity(
        Course course,
        string name,
        DateTime startDate,
        DateTime endDate,
        int maxStudents,
        CancellationToken cancellationToken
    )
    {
        try
        {
            course.UpdateDetails(name, startDate, endDate, maxStudents);
        
            return await courseRepository.Update(course, cancellationToken);
        }
        catch (Exception e)
        {
            return new CourseUnknownException(course.Id, e);
        }
      
    }
}