using Domain.Courses;
using Domain.Users;

namespace Domain.CourseUsers;

public class CourseUser
{
    public Guid CourseUserId { get; set; } = Guid.NewGuid();
    public CourseId CourseId { get; private set; }
    public Course? Course { get; }
    public UserId UserId { get; private set; }
    public User? User { get; }
    public decimal Rating { get; private set; }
    public bool IsFinished { get; private set; }

    private CourseUser(
        CourseId courseId,
        UserId userId,
        decimal rating,
        bool isFinished = false)
    {
        CourseId = courseId;
        UserId = userId;
        Rating = rating;
        IsFinished = isFinished;
    }
    public static CourseUser New(CourseId courseId, UserId userId, decimal rating, bool isFinished = false)
        => new(courseId, userId, rating, isFinished);
    public void FinishCourse()
    {
            IsFinished = true; 
    }

    public void SetRatingAndFinish(decimal rating)
    {
        Rating = rating;
        IsFinished = true;
    }
}