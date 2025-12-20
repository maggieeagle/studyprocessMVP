using Domain.Common;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }

        private Role() { }

        public Role(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name is required");

            Name = name;
        }
    }
}
