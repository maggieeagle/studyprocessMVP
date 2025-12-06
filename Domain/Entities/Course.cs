using Domain.Common;

namespace Domain.Entities
{
    public class Course : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

        public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

        public Course(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Course name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new DomainException("Course code is required.");
            Name = name.Trim();
            Code = code.Trim().ToUpperInvariant();
        }

        public void AddAssignment(Assignment assignment)
        {
            if (assignment == null) throw new ArgumentNullException(nameof(assignment));
            if (Assignments.Any(a => a.Title == assignment.Title && a.DueDate == assignment.DueDate))
                throw new DomainException("Identical assignment already exists.");

            Assignments.Add(assignment);
        }
    }
}
