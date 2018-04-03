using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace VoiceOnlineStats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel((options) =>
                {
                    if (Startup.CurrentEnvironment.IsDevelopment())
                    {
                        options.Listen(IPAddress.Loopback, 5000);
                    }

                    if (Startup.CurrentEnvironment.IsProduction())
                    {
                        options.Listen(IPAddress.Parse("94.142.139.47"), 80);
                    }
                })
                .UseStartup<Startup>()
                .UseSerilog(Log.Logger)
                .Build();
    }
}
