using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);
 
        builder.Property(l => l.Title)
            .IsRequired()
            .HasMaxLength(300);
 
        builder.Property(l => l.Content)
            .IsRequired();
    }
}
