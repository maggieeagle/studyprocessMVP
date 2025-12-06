using Domain.Entities;

namespace Domain.Events
{
    public class StudentEnrolledEvent : IDomainEvent
    {
        public Student Student { get; }
        public Course Course { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public StudentEnrolledEvent(Student student, Course course)
        {
            Student = student;
            Course = course;
        }
    }
}