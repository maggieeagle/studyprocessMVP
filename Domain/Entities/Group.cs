using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; private set; } = null!;
        public IReadOnlyCollection<UserGroup> Users => _users.AsReadOnly();

        private readonly List<UserGroup> _users = [];

        private Group() { } // for EF core

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

        internal void AddUser(User user)
        {
            if (_users.Any(ug => ug.UserId == user.Id)) return;

            var userGroup = new UserGroup(user, this);
            _users.Add(userGroup);

            // add to user's _groups if not already
            if (!user.Groups.Any(ug => ug.GroupId == this.Id))
                user._groups.Add(userGroup);
        }
    }
}
