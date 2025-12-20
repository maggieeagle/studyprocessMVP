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
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<UserGroup> UserGroup { get; set; }

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

            entity.HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(r => r.Name)
                .IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId);

            entity.HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(ug => new { ug.UserId, ug.GroupId });

            entity.HasOne(ug => ug.User)
                  .WithMany(u => u.Groups)
                  .HasForeignKey(ug => ug.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ug => ug.Group)
                  .WithMany(g => g.Users)
                  .HasForeignKey(ug => ug.GroupId)
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

        if (!Roles.Any())
        {
            Roles.AddRange(new Role("Student"), new Role("Teacher"));
        }

        if (!Users.Any())
        {
            var user = new User(new Email("a@a.a"), "123");
            user.CreateStudent("John", "Doe");

            var studentRole = Roles.Local.FirstOrDefault(r => r.Name == "Student") ?? Roles.First(r => r.Name == "Student");

            user.AddRole(studentRole);

            Users.Add(user);

            if (!Group.Any())
            {
                var mathGroup = new Group("Math Group");
                Group.Add(mathGroup);

                user.AssignToGroup(mathGroup);
            }
        }

        SaveChanges();
    }
}
