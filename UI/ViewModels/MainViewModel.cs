using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using UI.Views;

namespace UI.ViewModels
{
    public partial class MainViewModel(IAuthService authService) : ObservableObject
    {
        private readonly IAuthService _authService = authService;

        [ObservableProperty]
        private string _username = "";

        [ObservableProperty]
        private object? _currentView;

        public void Initialize()
        {
            _authService.StateChanged += OnAuthStateChanged;
            OnAuthStateChanged();
        }

        private void OnAuthStateChanged()
        {
            Username = _authService.GetCurrentUsername();

            if (string.IsNullOrEmpty(Username))
                NavigateToLogin();
            else
                NavigateToDashboard();
        }

        private static IServiceProvider Services => ((UI.App)System.Windows.Application.Current).Services;

        public void NavigateToLogin()
            => CurrentView = Services.GetRequiredService<LoginViewModel>();

        public void NavigateToRegister()
            => CurrentView = Services.GetRequiredService<RegisterViewModel>();

        private void NavigateToDashboard()
        {
            CurrentView = Services.GetRequiredService<StudentCoursesViewModel>();
        }

        public void NavigateToStudentCourses()
        {
            CurrentView = Services.GetRequiredService<StudentCoursesViewModel>();
        }

        [RelayCommand]
        private void SignOut()
        {
            _authService.SignOut();
        }

        [RelayCommand]
        private void OpenProfile()
        {
            var viewModel = Services.GetRequiredService<UserEditViewModel>();

            var window = new UserEditWindow(viewModel)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };

            window.ShowDialog();
        }
    }
}