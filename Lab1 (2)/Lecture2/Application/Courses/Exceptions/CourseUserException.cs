using Domain.Courses;
using Domain.Users;

namespace Application.Courses.Exceptions;

public abstract class CourseUserException(CourseId? courseId, UserId? userId, string message, Exception? innerException = null ) 
    : Exception(message, innerException )
{
    public CourseId? CourseId { get; } = courseId;
    public UserId? UserId { get; } = userId;
}
public class CoursesWithThisUserNotFoundException(UserId id) 
    : CourseUserException(null, id, $"User {id} do not registered in courses");
public class UsersInCourseNotFound(CourseId? courseId)
    :CourseUserException(courseId, null, $"Users in Course {courseId} not found");
public class UserWithCourseNotFoundException(CourseId courseId, UserId userId)
    :CourseUserException(courseId, userId, $"User {userId} in Course {courseId} not found");
public class CourseUserUnkownException(CourseId courseId, UserId? userId, Exception innerException)
    :CourseUserException(courseId, userId, $"Unkown exception for User {userId} and Course {courseId}", innerException);
public class CourseUserAlreadyExistsException(CourseId courseId, UserId userId)
    : CourseUserException(courseId, userId, $"User {userId} in Course {courseId} already exists");
public class CourseAlreadyFullException(CourseId courseId)
    : CourseUserException(courseId, null, $"Course {courseId} already full");
public class CourseUserNotFoundException(CourseId courseId)
    : CourseUserException(courseId, null, $"Course {courseId} not found");