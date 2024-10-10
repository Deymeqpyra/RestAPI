using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
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
    ICourseUserRepository courseUserRepository,
    IUserRepository userRepository,
    ICourseRepository courseRepository
)
    : IRequestHandler<AddUserToCourseCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        AddUserToCourseCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var userId = new UserId(request.UserId);
        var courseUserEntity =  await courseUserRepository.GetCourseByIds(courseId, userId, cancellationToken);
        var userList = await courseUserRepository.GetUsersByCourseId(courseId, cancellationToken);
        return await courseUserEntity.Match<Task<Result<CourseUser, CourseUserException>>>
        (
            async cu=> await Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserAlreadyExistsException(cu.CourseId, cu.UserId)),
            async () =>
            {
                var courseEntity = await courseRepository.GetById(courseId, cancellationToken);
                return await courseEntity.Match<Task<Result<CourseUser, CourseUserException>>>(
                    async c =>
                    {
                        var userEntity = await userRepository.GetById(userId, cancellationToken);
                        return await userEntity.Match<Task<Result<CourseUser, CourseUserException>>>(
                            async u =>
                            {
                                if (userList.Count >= c.MaxStudentsInCourse)
                                {
                                    return new CourseAlreadyFullException(courseId);
                                }

                                return await UpdateEntity(userId, courseId, cancellationToken);
                            },
                            async () => await Task.FromResult<CourseUserException>(
                                new CourseUserNotFoundUserException(userId)));

                    },
                    async () => await Task.FromResult<CourseUserException>(new CourseUserNotFoundCourseException(courseId))
                );
            }
        );
    }
    private async Task<Result<CourseUser, CourseUserException>> UpdateEntity(
        UserId userId,
        CourseId courseId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = CourseUser.New(courseId, userId, 1);
            return await courseUserRepository.Create(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new CourseUserUnkownException(CourseId.Empty(), UserId.Empty(), e);
        }
    }
}