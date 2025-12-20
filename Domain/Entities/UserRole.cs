namespace Domain.Entities
{
    public class UserRole
    {
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public int RoleId { get; private set; }
        public Role Role { get; private set; } = null!;

        private UserRole() { }

        internal UserRole(User user, Role role)
        {
            User = user;
            Role = role;
            UserId = user.Id;
            RoleId = role.Id;
        }
    }
}
