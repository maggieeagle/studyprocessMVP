using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.ValueObjects;

namespace UI.ViewModels
{
    public partial class RegisterViewModel(IAuthService authService, INavigationService navigationService) : ObservableObject
    {
        private readonly IAuthService _authService = authService;
        private readonly INavigationService _navigationService = navigationService;

        [ObservableProperty]
        private string _role = string.Empty;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _emailInput = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private string _successMessage = string.Empty;

        [RelayCommand]
        private void NavigateLogin()
        {
            _navigationService.NavigateToLogin();
        }

        [RelayCommand]
        private void Register()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Role))
            {
                ErrorMessage = "Please select your role (Student or Teacher)";
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Please enter your first and last name";
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailInput))
            {
                ErrorMessage = "Please enter your email address";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter a password";
                return;
            }

            try
            {
                var emailDomainObject = new Email(EmailInput);
                var userRegistrationDto = new UserRegistrationDto(
                            Role,
                            FirstName,
                            LastName,
                            Password,
                            emailDomainObject);

                bool success = _authService.Register(userRegistrationDto);

                if (!success)
                {
                    ErrorMessage = "Registration failed. This email may already be registered.";
                }
                else
                {
                    SuccessMessage = "Account created successfully! Redirecting to login...";
                    System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                        new Action(() => NavigateLogin()),
                        System.Windows.Threading.DispatcherPriority.Background);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Registration error: {ex.Message}";
            }
        }
    }
}
