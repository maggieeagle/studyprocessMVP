using Domain.Entities;

namespace Domain.Events
{
    public class StudentEnrolledEvent(Student student, Course course) : IDomainEvent
    {
        public Student Student { get; } = student;
        public Course Course { get; } = course;
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}