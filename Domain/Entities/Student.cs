using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Student : BaseEntity, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }

        public int? GroupId { get; private set; }
        public Group? Group { get; private set; }

        public ICollection<Grade> Grades { get; private set; } = new List<Grade>();
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();


        public Student(string firstName, string lastName, Email email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required");

            if (email is null)
                throw new ArgumentNullException(nameof(email));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }


        public void AssignToGroup(Group group)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            GroupId = group.Id;

            if (!group.Students.Contains(this))
                group.Students.Add(this);
        }


        public Enrollment EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (Enrollments.Any(e => e.CourseId == course.Id))
                throw new InvalidOperationException("Student is already enrolled in this course.");

            var enrollment = new Enrollment(this, course);
            Enrollments.Add(enrollment);

            _events.Add(new StudentEnrolledEvent(this, course));

            return enrollment;
        }


        public void AddGrade(Assignment assignment, decimal score)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            if (score < 0)
                throw new DomainException("Score cannot be negative.");

            if (assignment is HomeworkAssignment hw && score > hw.MaxPoints)
                throw new DomainException($"Score cannot exceed {hw.MaxPoints}.");

            var grade = new Grade(this, assignment, score);

            Grades.Add(grade);
        }
    }
}
