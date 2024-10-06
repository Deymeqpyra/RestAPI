using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using MediatR;

namespace Application.Courses.Commands;

public class CreateCourseCommand : IRequest<Result<Course, CourseException>>
{
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required string Name { get; init; }
    public required int MaxStudents  { get; init; }
}

public class CreateCourseCommandHandler(
    ICourseRepository courseRepository,
    ICourseQueries courseQueries)
    : IRequestHandler<CreateCourseCommand, Result<Course, CourseException>>
{
    public async Task<Result<Course, CourseException>> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var existingCourse = await courseQueries.SearchByName(request.Name, cancellationToken);
        
        return await existingCourse.Match(
            c => Task.FromResult<Result<Course, CourseException>>(new CourseAlreadyExistsException(c.Id)),
            async () => await CreateEntity(
                request.Name, 
                request.StartDate, 
                request.EndDate, 
                request.MaxStudents, 
                cancellationToken)
            );

    }

    public async Task<Result<Course, CourseException>> CreateEntity(
        string name,
        DateTime startDate,
        DateTime endDate,
        int maxStudents,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Course.New(CourseId.New(), name, startDate, endDate, maxStudents);
            return await courseRepository.Add(entity, cancellationToken);
        }
        catch (Exception e)
        {
           return new CourseUnknownException(CourseId.Empty(), e);
        }
    }
}
