namespace Domain.Courses;

public record CourseId(Guid Value)
{
    public static CourseId New() => new(Guid.NewGuid());
    public static CourseId Empty() => new(Guid.Empty);

    public override string ToString() => Value.ToString();
}