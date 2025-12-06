using Domain.Common;

namespace Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; private set; }
        public ICollection<Student> Students { get; private set; } = new List<Student>();

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
