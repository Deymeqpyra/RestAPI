using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using Optional;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository(ApplicationDbContext context) : ICourseRepository, ICourseQueries
{
    public async Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Courses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Course>> GetById(CourseId courseId, CancellationToken cancellationToken)
    {
        var entity = await context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == courseId, cancellationToken);
        
        return entity == null ? Option.None<Course>() : Option.Some(entity);
    }
    public async Task<Option<Course>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Name == name, cancellationToken);
        
        return entity == null ? Option.None<Course>() : Option.Some(entity);
    }

    
    public async Task<Course> Add(Course course, CancellationToken cancellationToken)
    {
        await context.Courses.AddAsync(course, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return course;
    }

    public async Task<Course> Update(Course course, CancellationToken cancellationToken)
    {
        context.Courses.Update(course);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return course;
    }

    public async Task<Course> Delete(Course course, CancellationToken cancellationToken)
    {
        var entity = await context.Courses.FirstOrDefaultAsync(x=>x.Id == course.Id, cancellationToken);
        context.Courses.Remove(entity);
        return entity;
    }
}