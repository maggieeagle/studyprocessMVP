using Domain.Common;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public Email Email { get; private set; } = null!;

        [Required]
        [MaxLength(64)]
        public string Password { get; private set; } = null!;

        public Student? Student { get; private set; }
        public Teacher? Teacher { get; private set; }

        public List<string> Roles { get; private set; } = [];

        private User() { }

        public User(Email email, string password)
        {
            Email = email;
            Password = password;
        }

        public Student CreateStudent(string firstName, string lastName)
        {
            Student = new Student(this, firstName, lastName);
            return Student;
        }

        public Teacher CreateTeacher(string firstName, string lastName)
        {
            Teacher = new Teacher(this, firstName, lastName);
            return Teacher;
        }

        public void AddRole(string role)
        {
            if (!string.IsNullOrWhiteSpace(role) && !Roles.Contains(role))
            {
                Roles.Add(role);
            }
        }
    }
}
