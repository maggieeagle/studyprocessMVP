using Domain.Common;
using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Group : BaseEntity
    {
        [Required]
        public string Name { get; private set; }
        public ICollection<Student> Students { get; private set; } = [];

        public Group(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Group name is required.");
            Name = name.Trim();
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new DomainException("Group name is required.");
            Name = newName.Trim();
        }
    }
}
