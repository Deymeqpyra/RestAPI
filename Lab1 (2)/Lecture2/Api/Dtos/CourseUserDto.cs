using Domain.CourseUsers;

namespace Api.Dtos;

public record CourseUserDto(
    Guid? CourseUserId,
    Guid CourseId,
    CourseDto Course,
    Guid UserId,
    UserDto User,
    decimal Rating,
    bool IsFinished
    )
{
    public static CourseUserDto FromCourseUser(CourseUser courseUser)
        => new(
            CourseUserId: courseUser.CourseUserId,
            CourseId: courseUser.CourseId.Value,
            Course: courseUser.Course == null ? null : CourseDto.FromDomainModel(courseUser.Course),
            UserId: courseUser.UserId.Value,
            User: courseUser.User == null ? null : UserDto.FromDomainModel(courseUser.User),
            Rating: courseUser.Rating,
            IsFinished: courseUser.IsFinished
        );
}