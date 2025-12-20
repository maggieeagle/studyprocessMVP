using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.ValueObjects;

namespace UI.ViewModels
{
    public partial class RegisterViewModel(IAuthService authService, INavigationService navigationService, IRoleRepository roleRepository) : ObservableObject
    {
        private readonly IAuthService _authService = authService;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IRoleRepository _roleRepository = roleRepository;

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
        private IReadOnlyList<string> _roles = [.. roleRepository.GetAll().Select(r => r.Name)];

        [RelayCommand]
        private void NavigateLogin()
        {
            _navigationService.NavigateToLogin();
        }

        [RelayCommand]
        private void Register()
        {
            if (string.IsNullOrWhiteSpace(EmailInput))
            {
                System.Windows.MessageBox.Show("Please enter an email address.");
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                System.Windows.MessageBox.Show("Please enter your first and last name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                System.Windows.MessageBox.Show("Please enter a password.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Role))
            {
                System.Windows.MessageBox.Show("Please select a role.");
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

                Roles = [.. _roleRepository.GetAll().Select(r => r.Name)];

                if (!success)
                {
                    System.Windows.MessageBox.Show("Registration failed. This email may already be registered.");
                }
                else
                {
                    System.Windows.MessageBox.Show("Registration successful!");
                    NavigateLogin();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Registration error: {ex.Message}");
            }
        }
    }
}
