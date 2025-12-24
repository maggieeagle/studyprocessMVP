using Application.Interfaces;
using Application.Services;
using Azure.Core;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Infrastructure
{
    // Dependency Injection for repositories
    public static class DI
    {

        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Repositories
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            //services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            // Application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStudentCourseService, StudentCourseService>();

            return services;
        }
    }
}
