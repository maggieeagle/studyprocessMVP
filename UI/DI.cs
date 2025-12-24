using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using UI.Services;
using UI.Views;
using UI.ViewModels;

namespace UI
{
    public static class DI
    {
        public static IServiceCollection RegisterUI(this IServiceCollection services)
        {
            // Navigation
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<StudentCoursesViewModel>(sp =>
            {
                return new StudentCoursesViewModel(
                    sp.GetRequiredService<INavigationService>(),
                    sp.GetRequiredService<IStudentCourseService>(),
                    sp.GetRequiredService<IAuthService>()
                );
            });

            services.AddSingleton<Func<int, CourseViewModel>>(sp => courseId =>
            {
                return new CourseViewModel(
                    courseId,
                    sp.GetRequiredService<ICourseRepository>(),
                    sp.GetRequiredService<IAuthService>(),
                    sp.GetRequiredService<INavigationService>()
                );
            });

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
