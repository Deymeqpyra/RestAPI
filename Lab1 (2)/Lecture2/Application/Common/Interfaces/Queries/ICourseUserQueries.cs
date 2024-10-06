using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;

namespace Application.Common.Interfaces.Queries;

public interface ICourseUserQueries
{
    Task<IReadOnlyList<CourseUser?>> GetCourseByUser(UserId userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersByCourseId(CourseId courseId, CancellationToken cancellationToken);
}