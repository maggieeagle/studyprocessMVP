using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/app-.log",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
