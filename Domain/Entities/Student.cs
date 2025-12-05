using System.Diagnostics;
using System.Text.RegularExpressions;

namespace App.Domain
{

    public class Student
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; } // unique
        public int? GroupId { get; private set; }
        public Group? Group { get; private set; }

        // Navigation properties
        public ICollection<Grade> Grades { get; private set; } = new List<Grade>();
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

        // Constructor
        public Student(string name, string email)
        {
            Name = name;
            Email = email;
        }

        // Methods for business logic
        public void AssignToGroup(Group group)
        {
            Group = group;
            GroupId = group.Id;
        }
    }
}