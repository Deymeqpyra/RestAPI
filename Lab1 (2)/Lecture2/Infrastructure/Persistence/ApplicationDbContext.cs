using System.Reflection;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Faculties;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseUser> CourseUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}