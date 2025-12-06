using Domain.Exceptions;

namespace Domain.Entities
{
    public class HomeworkAssignment : Assignment
    {
        public int MaxPoints { get; private set; }

        public HomeworkAssignment(string title, DateTime dueDate, Course course, int maxPoints) : base(title, dueDate, course)
        {
            if (maxPoints <= 0) throw new DomainException("MaxPoints must be positive.");
            MaxPoints = maxPoints;
        }
    }
}
