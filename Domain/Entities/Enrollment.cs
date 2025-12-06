namespace Domain.Entities
{
    public class Enrollment(Student student, Course course)
    {
        public int StudentId { get; private set; } = student.Id;
        public Student Student { get; private set; } = student ?? throw new ArgumentNullException(nameof(student));
        public int CourseId { get; private set; } = course.Id;
        public Course Course { get; private set; } = course ?? throw new ArgumentNullException(nameof(course));
        public DateTime EnrolledAt { get; private set; } = DateTime.UtcNow;
    }
}
