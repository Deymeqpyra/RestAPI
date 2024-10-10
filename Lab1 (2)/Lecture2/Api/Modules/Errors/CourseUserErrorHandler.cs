
using Application.Courses.Exceptions;
using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CourseUserErrorHandler
{
    public static ObjectResult ToObjectResult(this CourseUserException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                CoursesWithThisUserNotFoundException => StatusCodes.Status404NotFound,
                UserWithCourseNotFoundException => StatusCodes.Status404NotFound,
                UsersInCourseNotFound => StatusCodes.Status404NotFound,
                CourseUserUnkownException => StatusCodes.Status400BadRequest,
                CourseUserAlreadyExistsException => StatusCodes.Status409Conflict,
                CourseAlreadyFullException => StatusCodes.Status400BadRequest,
                CourseUserNotFoundCourseException => StatusCodes.Status404NotFound,
                CourseUserNotFoundUserException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}