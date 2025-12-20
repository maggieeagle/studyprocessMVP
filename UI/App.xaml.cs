using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Windows;
using UI.Services;
using UI.ViewModels;

namespace UI
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider Services { get; private set; } = null!;
        private static IConfiguration Configuration { get; set; } = null!;

        public App() { }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            Configuration = builder.Build();

            Services = ConfigureServices();

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

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(5, 7, 44))));

                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();

            services.AddTransient<INavigationService, NavigationService>();

            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}

