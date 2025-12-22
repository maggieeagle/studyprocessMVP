using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using UI.Services;
using UI.Views;
using UI.ViewModels;

namespace UI
{
    public static class DI
    {
        public static IServiceCollection RegisterUI(this IServiceCollection services, int studentId)
        {
            // Navigation
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<StudentCoursesViewModel>(sp =>
            {
                var navigationService = sp.GetRequiredService<INavigationService>();
                var studentCourseService = sp.GetRequiredService<IStudentCourseService>();
                return new StudentCoursesViewModel(navigationService, studentCourseService, studentId);
            });
            services.AddSingleton<Func<int, CourseViewModel>>(sp => courseId => new CourseViewModel(courseId, sp.GetRequiredService<ICourseRepository>()));

            // Pages
            services.AddTransient<CoursePage>();
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<StudentCoursesPage>();

            // Window
            services.AddSingleton<MainWindow>();

            return services;
        }
    }
}
