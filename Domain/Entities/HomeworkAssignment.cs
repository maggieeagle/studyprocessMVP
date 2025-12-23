using Domain.Exceptions;

namespace Domain.Entities
{
    public class HomeworkAssignment : Assignment
    {
        protected HomeworkAssignment() : base() { }  // for EF core
        public int MaxPoints { get; private set; }

        public HomeworkAssignment(string title, string description, DateTime dueDate, Course course, int maxPoints) : base(title, description, dueDate, course)
        {
            if (maxPoints <= 0) throw new DomainException("MaxPoints must be positive.");
            MaxPoints = maxPoints;
        }
    }
}
