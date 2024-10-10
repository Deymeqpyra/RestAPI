using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using MediatR;

namespace Application.Courses.Commands;

public class SetRatingCourseCommand : IRequest<Result<CourseUser, CourseUserException>>
{
    public required Guid UserId { get; init; }
    public required Guid CourseId { get; init; }
    public required decimal Rating { get; init; }
}

public class SetRatingCourseCommandHandler(
    ICourseUserRepository courseUserRepository
)
    : IRequestHandler<SetRatingCourseCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        SetRatingCourseCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var userId = new UserId(request.UserId);
        var entity = await courseUserRepository.GetCourseByIds(courseId, userId, cancellationToken);

        return await entity.Match(
            async cu => await UpdateRating(cu, request.Rating, cancellationToken),
            () => Task.FromResult<Result<CourseUser, CourseUserException>>(
                new UserWithCourseNotFoundException(courseId, userId)));
    }

    private async Task<Result<CourseUser, CourseUserException>> UpdateRating(
        CourseUser courseUser,
        decimal rating,
        CancellationToken cancellationToken)
    {
        try
        {
            courseUser.SetRatingAndFinish(rating);
            return await courseUserRepository.Update(courseUser, cancellationToken);
        }
        catch (Exception ex)
        {
            return new CourseUserUnkownException(courseUser.CourseId, courseUser.UserId, ex);
        }
    }
}