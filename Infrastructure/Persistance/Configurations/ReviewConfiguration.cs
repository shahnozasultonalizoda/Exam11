using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance;

public class ReviewConfiguration
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
 
        builder.Property(r => r.Rating)
            .IsRequired();
 
        builder.Property(r => r.Comment)
            .IsRequired()
            .HasMaxLength(2000);
 
        // Один студент — один отзыв на курс
        builder.HasIndex(r => new { r.StudentId, r.CourseId })
            .IsUnique();
 
        builder.HasOne(r => r.Student)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
