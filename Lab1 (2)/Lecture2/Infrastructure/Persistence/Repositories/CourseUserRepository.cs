using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CourseUserRepository(ApplicationDbContext context) : ICourseUserRepository, ICourseUserQueries
{
    public async Task<CourseUser> AddUserToCourse(
        CourseUser courseUser,
        CancellationToken cancellationToken)
    {
        await context.CourseUsers.AddAsync(courseUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return courseUser;
    }
    public async Task<CourseUser> UpdateCourseUser(CourseUser courseUser, CancellationToken cancellationToken)
    {
        context.CourseUsers.Update(courseUser);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return courseUser;
    }
    public async Task<Option<CourseUser>> GetCourseByIds(CourseId courseId, UserId userId, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.UserId == userId && x.CourseId == courseId, cancellationToken);
        
        return entity == null ? Option.None<CourseUser>() : Option.Some(entity);
    }
    public async Task<IReadOnlyList<CourseUser?>> GetCourseByUser(UserId userId, CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .Where(x=>x.UserId == userId)
            .ToListAsync(cancellationToken);
        
    }
    public async Task<IReadOnlyList<CourseUser>> GetAll(CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<List<CourseUser>> GetAllCourseUsersByCourseId(CourseId courseId, CancellationToken cancellationToken)
    {
        return await context.CourseUsers
            .AsNoTracking()
            .Where(x=>x.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<User>> GetUsersByCourseId(CourseId courseId, CancellationToken cancellationToken)
    {
        var entity = await context.CourseUsers
            .AsNoTracking()
            .Where(x => x.CourseId == courseId)
            .Select(x => x.User)
            .ToListAsync(cancellationToken);
        return entity;
    }

}