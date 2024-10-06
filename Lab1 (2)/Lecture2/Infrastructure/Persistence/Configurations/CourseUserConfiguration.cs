using System.Collections.Immutable;
using Domain.CourseUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourseUserConfiguration : IEntityTypeConfiguration<CourseUser>
{
    public void Configure(EntityTypeBuilder<CourseUser> builder)
    {
        builder.HasKey(x => x.CourseUserId);
        builder.HasOne(uc => uc.User)
            .WithMany(c=>c.CourseUsers) 
            .HasForeignKey(uc => uc.UserId)
            .HasConstraintName("fk_usercourse_user_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(uc => uc.Course)
            .WithMany(c=>c.CourseUsers) 
            .HasForeignKey(uc => uc.CourseId)
            .HasConstraintName("fk_usercourse_course_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(c=>c.Rating)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnType("decimal(18,2)");
        builder.Property(c=>c.IsFinished)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnType("bool");
        
    }
}