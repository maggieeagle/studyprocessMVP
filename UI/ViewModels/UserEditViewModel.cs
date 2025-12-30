using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Views;

namespace UI.ViewModels
{
    public partial class UserEditViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        public event EventHandler? OnRequestClose;

        [ObservableProperty] private string _firstName = string.Empty;
        [ObservableProperty] private string _lastName = string.Empty;
        [ObservableProperty] private string _emailInput = string.Empty;
        [ObservableProperty] private string _password = string.Empty;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private string _successMessage = string.Empty;

        public UserEditViewModel(IAuthService authService)
        {
            _authService = authService;
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = _authService.GetCurrentUser();
            if (user != null)
            {
                EmailInput = user.Email.Value;

                // Extract names from the associated profile
                if (user.Student != null)
                {
                    FirstName = user.Student.FirstName;
                    LastName = user.Student.LastName;
                }
                else if (user.Teacher != null)
                {
                    FirstName = user.Teacher.FirstName;
                    LastName = user.Teacher.LastName;
                }
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Name fields cannot be empty.";
                return;
            }

            bool success = _authService.UpdateProfile(FirstName, LastName, Password);

            if (success)
            {
                SuccessMessage = "Profile updated successfully!";
                // Brief delay so the user sees the success message before closing
                await Task.Delay(1000);
                OnRequestClose?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ErrorMessage = "Failed to update profile. Please try again.";
            }
        }

        [RelayCommand]
        private async Task Delete()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            var confirmed = CustomMessageBox.ShowConfirm(
                "Are you sure you want to permanently delete your account? This action cannot be undone.",
                "Confirm Deletion");

            if (confirmed)
            {
                bool success = _authService.DeleteCurrentAccount();

                if (success)
                {
                    SuccessMessage = "Account deleted successfully. Logging out...";

                    await Task.Delay(1500);

                    OnRequestClose?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Failed to delete account. Please try again later.";
                }
            }
        }
    }
}
