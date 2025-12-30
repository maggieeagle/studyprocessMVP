using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private static string? _currentUser;

        public event Action? StateChanged;

        private ILogger<AuthService> _logger;

        public AuthService(IUserRepository users, ILogger<AuthService> logger) 
        {
            _users = users;
            _logger = logger;
        }

        public bool Register(UserRegistrationDto details)
        {
            try
            {
                _logger.LogInformation("Attempting to register user with email {Email}", details.Email.Value);

                // Check if user already exists
                if (_users.GetByEmail(details.Email.Value) != null)
                {
                    _logger.LogWarning("Registration failed: user with email {Email} already exists", details.Email.Value);

                    return false;
                }

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

                _logger.LogInformation("User registered successfully with email {Email}", details.Email.Value);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for email {Email}", details.Email.Value);

                return false;
            }
        }

        public bool SignIn(UserDto details)
        {
            try
            {
                _logger.LogInformation("Sign-in attempt for email {Email}", details.Email);

                var user = _users.GetByEmailAndPassword(
                    details.Email,
                    details.Password);

                if (user == null)
                {
                    _logger.LogWarning("Sign-in failed for email {Email}", details.Email);

                    return false;
                }

                _currentUser = user.Email.Value;
                StateChanged?.Invoke();

                _logger.LogInformation("User signed in successfully: {Email}", user.Email.Value);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during sign-in for email {Email}", details.Email);

                return false;
            }
        }

        public void SignOut()
        {
            if (_currentUser != null)
            {
                _logger.LogInformation("User signed out: {Email}", _currentUser);
            }

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
                if (user == null)
                {
                    _logger.LogWarning("Update profile failed: no user logged in");
                    return false;
                }

                if (user.Student != null)
                    user.Student.UpdateName(firstName, lastName);
                else if (user.Teacher != null)
                    user.Teacher.UpdateName(firstName, lastName);

                if (!string.IsNullOrWhiteSpace(newPassword))
                    user.UpdatePassword(newPassword);

                _users.Update(user);

                _logger.LogInformation("Profile updated for user {Email}", user.Email.Value);

                StateChanged?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, "Error updating profile for user {Email}", _currentUser);

                System.Diagnostics.Debug.WriteLine($"Update Failed: {ex.Message}");
                return false;
            }
        }

        public bool DeleteCurrentAccount()
        {
            try
            {
                var user = GetCurrentUser();
                if (user == null)
                {
                    _logger.LogWarning("Delete account failed: no user logged in");

                    return false;
                }

                _users.Delete(user);

                _logger.LogInformation("User account deleted: {Email}", user.Email.Value);

                _currentUser = null;

                StateChanged?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account for user {Email}", _currentUser);

                return false;
            }
        }
    }
}