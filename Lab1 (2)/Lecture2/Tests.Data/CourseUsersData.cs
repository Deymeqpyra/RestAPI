using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;

namespace Tests.Data;

public static class CourseUsersData
{
    public static CourseUser MainCourseUser(CourseId courseId, UserId userId)
        => CourseUser.New(courseId, userId, 50);
}