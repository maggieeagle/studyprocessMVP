using Application.DTO;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        bool Register(UserRegistrationDto details);
        bool SignIn(UserDto details);
        void SignOut();
        string GetCurrentUsername();

        event Action StateChanged;
    }
}
