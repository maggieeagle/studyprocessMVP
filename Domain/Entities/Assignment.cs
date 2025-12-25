using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities
{
    public enum AssignmentStatus { Draft, Published, Closed }
    public abstract class Assignment : BaseEntity
    {
        public AssignmentStatus Status { get; private set; } = AssignmentStatus.Draft;
        public string Title { get; private set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public ICollection<Submission> Submissions { get; protected set; } = new List<Submission>();
        public ICollection<Grade> Grades { get; private set; } = [];

        protected Assignment() { } // for EF core
        protected Assignment(string title, string description, DateTime dueDate, Course course)
        {
            ArgumentNullException.ThrowIfNull(course);

            if (string.IsNullOrWhiteSpace(title)) throw new DomainException("Title required.");

            Title = title.Trim();
            Description = description;
            DueDate = dueDate;
            Course = course;
            CourseId = course.Id;
            Status = AssignmentStatus.Draft;
        }

        public void Publish()
        {
            if (Status != AssignmentStatus.Draft) throw new DomainException("Only draft assignments can be published.");
            Status = AssignmentStatus.Published;
        }

        public void Close()
        {
            if (Status == AssignmentStatus.Closed) throw new DomainException("Assignment already closed.");
            Status = AssignmentStatus.Closed;
        }

        public void AddGrade(Grade grade)
        {
            ArgumentNullException.ThrowIfNull(grade);

            if (DateTime.UtcNow > DueDate) throw new InvalidOperationException("Cannot grade assignment after due date.");
            Grades.Add(grade);
        }
    }
}
