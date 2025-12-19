using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UI.Services;
using UI.ViewModels;

namespace UI
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider Services { get; private set; } = null!;

        public App() { }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Services = ConfigureServices;

            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.SeedData();
            }

            var mainWindow = Services.GetRequiredService<MainWindow>();

            var mainViewModel = Services.GetRequiredService<MainViewModel>();
            mainViewModel.Initialize();

            mainWindow.Show();
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

