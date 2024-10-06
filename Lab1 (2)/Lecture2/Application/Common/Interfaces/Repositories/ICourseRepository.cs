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
}