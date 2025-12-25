using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        bool Register(UserRegistrationDto details);
        bool SignIn(UserDto details);
        void SignOut();
        int GetCurrentUserId();
        string[] GetCurrentUserRoles();
        string GetCurrentUsername();
        User? GetCurrentUser();
        bool UpdateProfile(string firstName, string lastName, string? newPassword);
        bool DeleteCurrentAccount();
        event Action StateChanged;
    }
}
