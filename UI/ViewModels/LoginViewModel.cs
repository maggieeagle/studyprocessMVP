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

        [RelayCommand]
        private void Login()
        {
            var userDto = new UserDto(Email, Password);

            bool success = _authService.SignIn(userDto);

            if (!success)
            {
                System.Windows.MessageBox.Show("Invalid login credentials");
            }
        }

        [RelayCommand]
        private void NavigateRegister()
        {
            _navigationService.NavigateToRegister();
        }
    }
}