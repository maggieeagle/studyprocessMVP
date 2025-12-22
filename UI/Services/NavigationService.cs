using Application.Interfaces;
using UI.ViewModels;

namespace UI.Services
{
    public class NavigationService(MainViewModel mainViewModel, Func<int, CourseViewModel> courseVmFactory) : INavigationService
    {
        private readonly MainViewModel _mainViewModel = mainViewModel;
        private readonly Func<int, CourseViewModel> _courseVmFactory = courseVmFactory;

        public void NavigateToLogin()
            => _mainViewModel.NavigateToLogin();

        public void NavigateToRegister()
            => _mainViewModel.NavigateToRegister();

        public void NavigateToStudentCourses()
            => _mainViewModel.NavigateToStudentCourses();
        public void NavigateToCourseDetails(int courseId)
        {
            var vm = _courseVmFactory(courseId);
            _mainViewModel.CurrentView = vm;
        }
    }
}
