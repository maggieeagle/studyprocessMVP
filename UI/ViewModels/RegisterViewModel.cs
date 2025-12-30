using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.ValueObjects;
using UI.Views;

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
                ErrorMessage = Resource1.RegisterRoleError;
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = Resource1.RegisterNameError;
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailInput))
            {
                ErrorMessage = Resource1.RegisterEmailAddress;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = Resource1.RegisterPasswordError;
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
                    ErrorMessage = Resource1.RegistrationFailError;
                }
                else
                {
                    SuccessMessage = Resource1.RegistrationSuccess;
                    System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                        new Action(() => NavigateLogin()),
                        System.Windows.Threading.DispatcherPriority.Background);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format(Resource1.RegistrationError, ex.Message);
            }
        }
    }
}
