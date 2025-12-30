using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Enrollment
    {
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [Required]
        public DateTime EnrolledAt { get; set; }

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
