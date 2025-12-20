using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<HomeworkAssignment> HomeworkAssignments { get; set; }
    public DbSet<ExamAssignment> ExamAssignments { get; set; }

    public class EmailConverter() : ValueConverter<Email, string>(
        v => v.Value,
        v => new Email(v));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var emailConverter = new EmailConverter();

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<HomeworkAssignment>();
        modelBuilder.Entity<ExamAssignment>();


        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Email)
                .HasConversion(emailConverter)
                .HasMaxLength(320)
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();

            // EF Core cannot store List<string> directly. 
            // TODO: Hold roles in separate table
            entity.Property(u => u.Roles)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            entity.HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Ignore(t => t.Email);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Ignore(s => s.Email);
        });

        modelBuilder.Entity<Course>()
           .HasIndex(c => c.Code)
           .IsUnique();
    }


    public void SeedData()
    {
        Database.EnsureCreated();

        if (!Users.Any())
        {
            var user = new User(new Email("a@a.a"), "123");
            user.CreateStudent("John", "Doe");

            Users.Add(user);
            SaveChanges();
        }
    }
}
