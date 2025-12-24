using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AuthService(IUserRepository users) : IAuthService
    {
        private readonly IUserRepository _users = users;
        private static string? _currentUser;

        public event Action? StateChanged;

        public bool Register(UserRegistrationDto details)
        {
            try
            {
                // Check if user already exists
                if (_users.GetByEmail(details.Email.Value) != null)
                    return false;

                var user = new User(details.Email, details.Password);
                user.AddRole(details.Role);

                if (details.Role.Equals("Student"))
                {
                    user.CreateStudent(details.FirstName, details.LastName);
                } else if (details.Role.Equals("Teacher"))
                {
                    user.CreateTeacher(details.FirstName, details.LastName);
                }

                _users.Add(user);
                return true;
            }
            catch (Exception)
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

        public int GetCurrentUserId()
        {
            var user = _users.GetByEmail(_currentUser ?? "");
            return user?.Id ?? 0;
        }

        public string[] GetCurrentUserRoles()
        {
            var user = _users.GetByEmail(_currentUser ?? "");
            return user?.Roles?.ToArray() ?? Array.Empty<string>();
        }
    }
}