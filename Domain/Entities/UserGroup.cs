namespace Domain.Entities
{
    public class UserGroup
    {
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public int GroupId { get; private set; }
        public Group Group { get; private set; } = null!;

        private UserGroup() { } // for EF core

        public UserGroup(User user, Group group)
        {
            User = user;
            Group = group;
            UserId = user.Id;
            GroupId = group.Id;
        }
    }
}
