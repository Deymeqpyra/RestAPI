using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<Course> Add(Course course, CancellationToken cancellationToken);
    Task<Course> Update(Course course, CancellationToken cancellationToken);
    Task<Course> Delete(Course course, CancellationToken cancellationToken);
    Task<Option<Course>> GetById(CourseId id, CancellationToken cancellationToken);
    Task<Option<CourseUser>> GetCourseByIds(CourseId courseId, UserId userId, CancellationToken cancellationToken);
    Task<CourseUser> UpdateCourseUser(CourseUser courseUser, CancellationToken cancellationToken);
    Task<CourseUser> AddUserToCourse(
        CourseUser courseUser,
        CancellationToken cancellationToken);
}