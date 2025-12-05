namespace App.Domain
{
    public class Course
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; } // unique

        // Navigation
        public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

        public Course(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
