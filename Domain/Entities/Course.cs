using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string TeacherName { get; set; }

        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public ICollection<Assignment> Assignments { get; private set; } = [];
        public ICollection<Enrollment> Enrollments { get; private set; } = [];

        public Course(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Course name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new DomainException("Course code is required.");

            Name = name.Trim();
            Code = code.Trim().ToUpperInvariant();
        }

        public void AddAssignment(Assignment assignment)
        {
            ArgumentNullException.ThrowIfNull(assignment);

            if (Assignments.Any(a => a.Title == assignment.Title && a.DueDate == assignment.DueDate))
                throw new DomainException("Identical assignment already exists.");

            Assignments.Add(assignment);
        }
    }
}
