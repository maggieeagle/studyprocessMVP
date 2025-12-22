using Domain.Common;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Student : BaseEntity
    {
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public Email Email { get; private set; } = null!;

        public int? GroupId { get; private set; }
        public Group? Group { get; private set; }

        public ICollection<Grade> Grades { get; private set; } = [];
        public ICollection<Enrollment> Enrollments { get; private set; } = [];

        private readonly List<IDomainEvent> _events = [];
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        private Student() { } // for EF core
        internal Student(User user, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required");

            User = user;
            UserId = user.Id;
            FirstName = firstName;
            LastName = lastName;
            Email = user.Email;
        }


        public void AssignToGroup(Group group)
        {
            ArgumentNullException.ThrowIfNull(nameof(group));

            Group = group;
            GroupId = group.Id;

            if (!group.Students.Contains(this))
                group.Students.Add(this);
        }


        public Enrollment EnrollInCourse(Course course)
        {
            ArgumentNullException.ThrowIfNull(course);

            if (Enrollments.Any(e => e.CourseId == course.Id)) throw new InvalidOperationException("Student is already enrolled in this course.");

            var enrollment = new Enrollment(this, course);
            Enrollments.Add(enrollment);

            _events.Add(new StudentEnrolledEvent(this, course));

            return enrollment;
        }


        public void AddGrade(Assignment assignment, decimal score)
        {
            ArgumentNullException.ThrowIfNull(assignment);

            if (score < 0) throw new DomainException("Score cannot be negative.");
            if (assignment is HomeworkAssignment hw && score > hw.MaxPoints) throw new DomainException($"Score cannot exceed {hw.MaxPoints}.");

            var grade = new Grade(this, assignment, score);

            Grades.Add(grade);
        }
    }
}
