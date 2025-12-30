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
                ErrorMessage = Resource1.UserEditNameError;
                return;
            }

            bool success = _authService.UpdateProfile(FirstName, LastName, Password);

            if (success)
            {
                SuccessMessage = Resource1.UserEditSuccess;
                // Brief delay so the user sees the success message before closing
                await Task.Delay(1000);
                OnRequestClose?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ErrorMessage = Resource1.EditUserFail;
            }
        }

        [RelayCommand]
        private async Task Delete()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            var confirmed = CustomMessageBox.ShowConfirm(
                Resource1.UserDeleteConfirm,
                Resource1.ConfirmDeletion);

            if (confirmed)
            {
                bool success = _authService.DeleteCurrentAccount();

                if (success)
                {
                    SuccessMessage = Resource1.UserDeleteSuccess;

                    await Task.Delay(1500);

                    OnRequestClose?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = Resource1.UserDeleteError;
                }
            }
        }
    }
}
