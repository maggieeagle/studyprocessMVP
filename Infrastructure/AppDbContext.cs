using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<HomeworkAssignment> HomeworkAssignments { get; set; }
    public DbSet<ExamAssignment> ExamAssignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }

    public class EmailConverter() : ValueConverter<Email, string>(
        v => v.Value,
        v => new Email(v));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var emailConverter = new EmailConverter();

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Student)
        .WithMany(s => s.Enrollments)
        .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);

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

        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId);
    }


    public void SeedData()
    {
        Database.EnsureCreated();

        if (!Users.Any())
        {
            // create student
            var user1 = new User(new Email("john.doe@ut.ee"), "123");
            user1.CreateStudent("John", "Doe");

            //create teacher
            var user2 = new User(new Email("jane.doe@ut.ee"), "123");
            user2.CreateTeacher("Jane", "Doe");

            Users.Add(user1);
            Users.Add(user2);
            SaveChanges();
        }
        if (!Courses.Any())
        {
            var course1 = new Course("Mathematics", "MATH101", DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(1)) { TeacherName = "Doctor" };
            var course2 = new Course("History", "HIST101", DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(1)) { TeacherName = "Teacher" };
            var course3 = new Course("Physics", "PHYS101", DateTime.Today.AddMonths(-11), DateTime.Today.AddMonths(-6)) { TeacherName = "Doctor" };

            Courses.AddRange(course1, course2, course3);
            SaveChanges();

            if (!HomeworkAssignments.Any())
            {
                var assignment1 = new HomeworkAssignment("Algebra Quiz", "-", DateTime.Now.AddDays(-2), course1, 10);
                var assignment2 = new HomeworkAssignment("Final Project", "-", DateTime.Now.AddDays(-2), course1, 10);
                var assignment3 = new ExamAssignment("Calculus Exam", "-", DateTime.Now.AddDays(2), course2, DateTime.Now.AddDays(2));

                Assignments.AddRange(assignment1, assignment2, assignment3);
                SaveChanges();
            }
        }
    }
}
