using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using Domain.CourseUsers;
using MediatR;
using Optional;

namespace Application.Courses.Commands;

public class FinishCourseForUsersCommand : IRequest<Result<IReadOnlyList<CourseUser>, CourseUserException>>
{
    public required Guid CourseId { get; init; }
}

public class FinishCourseForUsersCommandHandler(
    ICourseUserRepository courseUserRepository,
    ICourseRepository courseRepository
) : IRequestHandler<FinishCourseForUsersCommand, Result<IReadOnlyList<CourseUser>, CourseUserException>>
{
    public async Task<Result<IReadOnlyList<CourseUser>, CourseUserException>> Handle(
        FinishCourseForUsersCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var courseEntity = await courseRepository.GetById(courseId, cancellationToken);
        
        return await courseEntity.Match<Task<Result<IReadOnlyList<CourseUser>, CourseUserException>>>(
            async cu => await UpdateEntity(cu, cancellationToken),
            async () => await Task.FromResult<CourseUserException>(new CourseUserNotFoundCourseException(courseId))) ;
    }

    public async Task<Result<IReadOnlyList<CourseUser>, CourseUserException>> UpdateEntity
        (Course course, CancellationToken cancellationToken)
    {
        try
        {
            var courseList = await courseUserRepository.GetAllCourseUsersByCourseId(course.Id, cancellationToken);
            foreach (var courseItem in courseList)
            {
                courseItem.FinishCourse();
            }
            await courseUserRepository.UpdateRange(courseList, cancellationToken);
            return courseList;
        }
        catch (Exception e)
        {
            return new CourseUserUnkownException(course.Id, null, e);
        }
    }
}
