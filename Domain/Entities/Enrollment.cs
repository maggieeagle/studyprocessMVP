namespace App.Domain
{
    public class Enrollment
    {
        public int StudentId { get; private set; }
        public Student Student { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }

        public Enrollment(Student student, Course course)
        {
            Student = student;
            StudentId = student.Id;
            Course = course;
            CourseId = course.Id;
        }
    }
}
