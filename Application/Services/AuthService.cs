using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AuthService(IUserRepository users, IRoleRepository roles) : IAuthService
    {
        private readonly IUserRepository _users = users;
        private readonly IRoleRepository _roles = roles;

        private static string? _currentUser;

        public event Action? StateChanged;

        public bool Register(UserRegistrationDto details)
        {
            try
            {
                if (_users.GetByEmail(details.Email.Value) != null)
                    return false;

                var user = new User(details.Email, details.Password);

                var role = _roles.GetByName(details.Role)
                    ?? throw new Exception("Role not found");

                user.AddRole(role);

                if (details.Role == "Student")
                    user.CreateStudent(details.FirstName, details.LastName);
                else if (details.Role == "Teacher")
                    user.CreateTeacher(details.FirstName, details.LastName);

                _users.Add(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SignIn(UserDto details)
        {
            try
            {
                var user = _users.GetByEmailAndPassword(
                    details.Email,
                    details.Password);

                if (user == null)
                    return false;

                _currentUser = user.Email.Value;
                StateChanged?.Invoke();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SignOut()
        {
            _currentUser = null;
            StateChanged?.Invoke();
        }

        public string GetCurrentUsername() => _currentUser ?? "";
    }
}