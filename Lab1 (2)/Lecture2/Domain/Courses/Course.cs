using Domain.CourseUsers;
using Domain.Users;

namespace Domain.Courses;

public class Course
{
    public CourseId Id { get; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Name { get; private set; }
    public int MaxStudents { get; private set; }

    public ICollection<CourseUser> CourseUsers { get; set; }
    
    public int MaxStudentsInCourse => MaxStudents;
    private Course(
        CourseId id, 
        string name, 
        DateTime startDate, 
        DateTime endDate, 
        int maxStudents) 
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Name = name;
        MaxStudents = maxStudents;        
    }
    
    public static Course New(CourseId id, string name, DateTime startDate, DateTime endDate, int maxStudents)
        => new(id, name, startDate, endDate, maxStudents);

    public void UpdateDetails(string name, DateTime startDate, DateTime endDate, int maxStudents)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        MaxStudents = maxStudents;
    }   
    
}