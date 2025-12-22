using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UI.Views;

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
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();   // or db.Database.Migrate(); for final version
                db.SeedData();                 // seed courses and users
            }

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            // Show student courses page
            var studentCoursesPage = Services.GetRequiredService<StudentCoursesPage>();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.RegisterInfrastructure(connectionString);
            services.RegisterUI(studentId: 1);

            return services.BuildServiceProvider();
        }
    }
}

