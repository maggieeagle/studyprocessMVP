namespace Domain.Entities
{
    public class Enrollment
    {
        public int StudentId { get; private set; }
        public Student Student { get; private set; } = null!;

        public int CourseId { get; private set; }
        public Course Course { get; private set; } = null!;

        public DateTime EnrolledAt { get; private set; }

        protected Enrollment() { }  // for EF core

        public Enrollment(Student student, Course course)
        {
            Student = student ?? throw new ArgumentNullException(nameof(student));
            Course = course ?? throw new ArgumentNullException(nameof(course));

            StudentId = student.Id;
            CourseId = course.Id;

            EnrolledAt = DateTime.UtcNow;
        }
    }
}
