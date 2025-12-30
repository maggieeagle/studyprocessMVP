using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UI.ViewModels
{
    public partial class LoginViewModel(IAuthService authService, INavigationService navigationService) : ObservableObject
    {
        private readonly IAuthService _authService = authService;
        private readonly INavigationService _navigationService = navigationService;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [RelayCommand]
        private void Login()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = Resource1.LoginEmailError;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = Resource1.LoginPasswordError;
                return;
            }

            var userDto = new UserDto(Email, Password);

            bool success = _authService.SignIn(userDto);

            if (!success)
            {
                ErrorMessage = Resource1.LoginInvalidCredentialsError;
            }
        }

        [RelayCommand]
        private void NavigateRegister()
        {
            _navigationService.NavigateToRegister();
        }
    }
}