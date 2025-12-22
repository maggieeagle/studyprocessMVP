using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UI.Services;
using UI.ViewModels;
using UI.Views;

namespace UI
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider Services { get; private set; } = null!;

        public App() { }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=StudyProcessMVP;Trusted_Connection=True;";
            int currentStudentId = 1; // from login/auth

            services.RegisterInfrastructure(connectionString);
            
            services.AddSingleton<MainViewModel>();
            services.RegisterUI(currentStudentId);

            services.AddSingleton<MainWindow>();

            var serviceProvider = services.BuildServiceProvider();
            Services = serviceProvider;

            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();   // or db.Database.Migrate(); for final version
                db.SeedData();                 // seed courses and users
            }

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            // Show student courses page
            var studentCoursesPage = serviceProvider.GetRequiredService<StudentCoursesPage>();
        }

        private static IServiceProvider ConfigureServices
        {
            get
            {
                {
                    var services = new ServiceCollection();

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(
                            @"Server=(localdb)\MSSQLLocalDB;Database=StudyProcessMVP;Trusted_Connection=True;"));

                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IAuthService, AuthService>();

                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<RegisterViewModel>();

                    services.AddTransient<INavigationService, NavigationService>();

                    services.AddTransient<MainWindow>();

                    return services.BuildServiceProvider();
                }
            }
        }
    }
}

