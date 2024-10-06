using Domain.Courses;
using Domain.CourseUsers;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c=>c.Id).HasConversion(c=>c.Value, c=>new CourseId(c));
        
        builder.Property(c => c.MaxStudents).IsRequired().HasColumnType("SMALLINT");
        builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(255)");

        builder.Property(c => c.StartDate)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        builder.Property(c => c.EndDate)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        // builder.OwnsMany(x=>x.Users, b=>{})
        // builder.OwnsOne()
    }
}