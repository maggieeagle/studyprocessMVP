namespace Application.Interfaces
{
    public interface INavigationService
    {
        void NavigateToRegister();
        void NavigateToLogin();
        void NavigateToStudentCourses();
        void NavigateToCourseDetails(int courseId);
    }
}
