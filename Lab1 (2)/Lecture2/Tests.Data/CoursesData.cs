using Domain.Courses;

namespace Tests.Data;

public static class CoursesData
{
    public static Course MainCourse 
        => Course.New(CourseId.New(), 
            "CourseTest",
            DateTime.UtcNow, 
            DateTime.UtcNow.AddYears(1), 
            5); 
    public static Course ExtraCourse 
        => Course.New(CourseId.New(), 
            "ExtraCourseTest",
            DateTime.UtcNow, 
            DateTime.UtcNow.AddYears(1), 
            10); 
}