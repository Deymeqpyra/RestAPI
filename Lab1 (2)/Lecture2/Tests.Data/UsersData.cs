using Domain.Faculties;
using Domain.Users;

namespace Tests.Data;

public static class UsersData
{
    public static User MainUser(FacultyId facultyId)
    => User.New(
        UserId.New(), 
        "User First Name 1",
        "User Last name 1",
        facultyId);
    public static User ExtraUser(FacultyId facultyId)
        => User.New(
            UserId.New(), 
            "Extra User First Name 1",
            "Extra User Last name 1",
            facultyId);
}