using Domain.Courses;
using Domain.CourseUsers;
using Domain.Faculties;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICourseQueries
{
    Task<Option<Course>> SearchByName(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken);
    Task<Option<Course>> GetById(CourseId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<CourseUser?>> GetCourseByUser(UserId userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersByCourseId(CourseId courseId, CancellationToken cancellationToken);
}