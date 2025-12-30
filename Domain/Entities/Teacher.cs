using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities
{
    public class Teacher : BaseEntity
    {
        [Required]
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        [Required]
        public string FirstName { get; private set; } = null!;
        [Required]
        public string LastName { get; private set; } = null!;
        public Email Email => User.Email;
        public int? GroupId { get; private set; }
        public Group? Group { get; private set; }

        public ICollection<Grade> Grades { get; private set; } = [];
        public ICollection<Enrollment> Enrollments { get; private set; } = [];

        private readonly List<IDomainEvent> _events = [];
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        private Teacher() { } // for EF core
        internal Teacher(User user, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required");

            User = user;
            UserId = user.Id;
            FirstName = firstName;
            LastName = lastName;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("First and last names are required.");

            FirstName = firstName;
            LastName = lastName;
        }
    }
}
