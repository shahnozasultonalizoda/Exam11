using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
   public DbSet<Student> Students { get; set; }
   public DbSet<Lesson> Lessons {get; set;}
   public DbSet<Course> Courses { get; set; }
   public DbSet<Enrollment> Enrollments {get; set;}
   public DbSet<Review> Reviews { get; set; }
   public DbSet<Category> Categories {get; set;}
   public DbSet<User> Users { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
