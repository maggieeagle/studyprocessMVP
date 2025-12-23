using Application.DTO;

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

        event Action StateChanged;
    }
}
