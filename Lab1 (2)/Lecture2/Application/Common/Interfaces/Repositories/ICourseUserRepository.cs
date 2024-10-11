using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseUserRepository
{
    Task<Option<CourseUser>> GetCourseByIds(CourseId courseId, UserId userId, CancellationToken cancellationToken);
    Task<CourseUser> Update(CourseUser courseUser, CancellationToken cancellationToken);
    Task<CourseUser> Create(
        CourseUser courseUser,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersByCourseId(CourseId courseId, CancellationToken cancellationToken);
    Task<List<CourseUser>> GetAllCourseUsersByCourseId
        (CourseId courseId, CancellationToken cancellationToken);
    Task<IReadOnlyList<CourseUser>> GetAll(CancellationToken cancellationToken);

    Task<IReadOnlyList<CourseUser>> UpdateRange(IReadOnlyList<CourseUser> courseUser,
        CancellationToken cancellationToken);
}