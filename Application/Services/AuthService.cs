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

        public User? GetCurrentUser()
        {
            if (string.IsNullOrEmpty(_currentUser)) return null;
            return _users.GetByEmail(_currentUser);
        }

        public bool UpdateProfile(string firstName, string lastName, string? newPassword)
        {
            try
            {
                var user = _users.GetByEmail(_currentUser ?? "");
                if (user == null) return false;

                if (user.Student != null)
                    user.Student.UpdateName(firstName, lastName);
                else if (user.Teacher != null)
                    user.Teacher.UpdateName(firstName, lastName);

                if (!string.IsNullOrWhiteSpace(newPassword))
                    user.UpdatePassword(newPassword);

                _users.Update(user);

                StateChanged?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update Failed: {ex.Message}");
                return false;
            }
        }

        public bool DeleteCurrentAccount()
        {
            try
            {
                var user = GetCurrentUser();
                if (user == null) return false;

                _users.Delete(user);
                _currentUser = null;

                StateChanged?.Invoke();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}