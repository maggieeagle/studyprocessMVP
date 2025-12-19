using Application.Interfaces;
using UI.ViewModels;

namespace UI.Services
{
    public class NavigationService(MainViewModel mainViewModel) : INavigationService
    {
        private readonly MainViewModel _mainViewModel = mainViewModel;

        public void NavigateToLogin()
            => _mainViewModel.NavigateToLogin();

        public void NavigateToRegister()
            => _mainViewModel.NavigateToRegister();
    }
}
