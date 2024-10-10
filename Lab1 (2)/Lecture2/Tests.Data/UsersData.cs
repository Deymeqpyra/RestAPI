using Domain.Faculties;
using Domain.Users;

namespace Tests.Data;

public static class UsersData
{
    public static User MainUser(FacultyId facultyId)
    => User.New(UserId.New(), "User First Name 1", "User Last name 1", facultyId);
}