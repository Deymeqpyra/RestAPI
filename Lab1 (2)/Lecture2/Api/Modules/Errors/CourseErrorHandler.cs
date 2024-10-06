using System.Diagnostics.Metrics;
using Application.Courses.Exceptions;
using Application.Faculties.Exceptions;
using Domain.Courses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CourseErrorHandler
{
    public static ObjectResult ToObjectResult(this CourseException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                CourseNotFoundException => StatusCodes.Status404NotFound,
                CourseAlreadyExistsException => StatusCodes.Status409Conflict,
                CourseUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Faculty error handler does not implemented")
            }
        };
    }
}