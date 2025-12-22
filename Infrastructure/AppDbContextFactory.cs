using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use your database provider here, e.g., LocalDB or SQLite
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StudyProcessMVP;Trusted_Connection=True;\r\n");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
