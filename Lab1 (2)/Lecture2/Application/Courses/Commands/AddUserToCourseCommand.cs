using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Application.Users.Exceptions;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using MediatR;

namespace Application.Courses.Commands;

public class AddUserToCourseCommand : IRequest<Result<CourseUser, CourseUserException>>
{ 
    public required Guid CourseId { get; init; }
    public required Guid UserId { get; init; }
}

public class AddUserToCourseCommandHandler(
    ICourseUserRepository courseUserRepository
)
    : IRequestHandler<AddUserToCourseCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        AddUserToCourseCommand request,
        CancellationToken cancellationToken)
    {
        var courseID = new CourseId(request.CourseId);
        var userId = new UserId(request.UserId);
        var courseUserEntity =  await courseUserRepository.GetCourseByIds(courseID, userId, cancellationToken);
        return await courseUserEntity.Match<Task<Result<CourseUser, CourseUserException>>>(
            async cu => await Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserAlreadyExistsException(cu.CourseId, cu.UserId)),
            async () => await UpdateEntity(userId, courseID, cancellationToken));
    }
    private async Task<Result<CourseUser, CourseUserException>> UpdateEntity(
        UserId userId,
        CourseId courseId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = CourseUser.New(courseId, userId, 1);
            return await courseUserRepository.AddUserToCourse(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new CourseUserUnkownException(CourseId.Empty(), UserId.Empty(), e);
        }
    }
}