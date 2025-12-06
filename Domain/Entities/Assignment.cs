using Domain.Common;

namespace Domain.Entities
{
    public enum AssignmentStatus { Draft, Published, Closed }
    public abstract class Assignment : BaseEntity
    {
        public AssignmentStatus Status { get; private set; } = AssignmentStatus.Draft;
        public string Title { get; private set; }
        public DateTime DueDate { get; private set; }

        public int CourseId { get; private set; }
        public Course Course { get; private set; }

        public ICollection<Grade> Grades { get; private set; } = new List<Grade>();

        protected Assignment(string title, DateTime dueDate, Course course)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title required.");

            if (course == null)
                throw new ArgumentNullException(nameof(course));

            Title = title.Trim();
            DueDate = dueDate;
            Course = course;
            CourseId = course.Id;
            Status = AssignmentStatus.Draft;
        }

        public void Publish()
        {
            if (Status != AssignmentStatus.Draft)
                throw new DomainException("Only draft assignments can be published.");
            Status = AssignmentStatus.Published;
        }

        public void Close()
        {
            if (Status == AssignmentStatus.Closed)
                throw new DomainException("Assignment already closed.");
            Status = AssignmentStatus.Closed;
        }

        public void AddGrade(Grade grade)
        {
            if (DateTime.UtcNow > DueDate)
                throw new InvalidOperationException("Cannot grade assignment after due date.");

            if (grade == null)
                throw new ArgumentNullException(nameof(grade));

            Grades.Add(grade);
        }
    }
}
