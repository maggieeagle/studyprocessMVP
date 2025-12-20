using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public Email Email { get; private set; } = null!;
        public string Password { get; private set; } = null!;

        public Student? Student { get; private set; }
        public Teacher? Teacher { get; private set; }

        private readonly List<UserRole> _roles = [];
        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

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

        public void AddRole(Role role)
        {
            if (_roles.Any(r => r.Role.Name == role.Name))
                return;

            _roles.Add(new UserRole(this, role));
        }
    }
}
