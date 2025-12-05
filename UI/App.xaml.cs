using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace UI
{
    public partial class App : System.Windows.Application
    {
        public static AppDbContext DbContext { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=StudyProcessMVP;Trusted_Connection=True;")
                .Options;

            DbContext = new AppDbContext(options);

            DbContext.Database.EnsureCreated(); // Create database if it doesn't exist
        }
    }
}
