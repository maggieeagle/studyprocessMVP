using Domain.Common;
using Domain.Exceptions;
using System.Text;

namespace Domain.Entities
{
    public class Course : BaseEntity
    {
        public enum CourseStatus
        {
            Available,
            Enrolled,
            Completed
        }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public CourseStatus Status { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public ICollection<Assignment> Assignments { get; private set; } = [];
        public ICollection<Enrollment> Enrollments { get; private set; } = [];

        public Course(string name, string code, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Course name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new DomainException("Course code is required.");
            if (startDate > endDate)
                throw new DomainException("Start date cannot be after end date.");

            Name = name.Trim();
            Code = code.Trim().ToUpperInvariant();
            Status = CourseStatus.Available;
        }

        public void AddAssignment(Assignment assignment)
        {
            ArgumentNullException.ThrowIfNull(assignment);

            if (Assignments.Any(a => a.Title == assignment.Title && a.DueDate == assignment.DueDate))
                throw new DomainException("Identical assignment already exists.");

            Assignments.Add(assignment);
        }

        public void Enroll()
        {
            if (Status != CourseStatus.Available)
                throw new DomainException("Only available courses can be enrolled.");

            Status = CourseStatus.Enrolled;
        }

        public void Complete()
        {
            if (Status != CourseStatus.Enrolled)
                throw new DomainException("Only enrolled courses can be completed.");

            Status = CourseStatus.Completed;
        }
    }
}
