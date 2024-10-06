﻿using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseUserRepository
{
    Task<Option<CourseUser>> GetCourseByIds(CourseId courseId, UserId userId, CancellationToken cancellationToken);
    Task<CourseUser> UpdateCourseUser(CourseUser courseUser, CancellationToken cancellationToken);
    Task<CourseUser> AddUserToCourse(
        CourseUser courseUser,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersByCourseId(CourseId courseId, CancellationToken cancellationToken);
}