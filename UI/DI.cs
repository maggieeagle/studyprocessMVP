using Application.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
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
                var studentCourseService = sp.GetRequiredService<IStudentCourseService>();
                return new StudentCoursesViewModel(studentCourseService, studentId);
            });

            // Pages
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<StudentCoursesPage>();

            // Window
            services.AddSingleton<MainWindow>();

            return services;
        }
    }
}
