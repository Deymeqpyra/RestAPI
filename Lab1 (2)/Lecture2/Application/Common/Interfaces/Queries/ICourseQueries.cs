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
    
}