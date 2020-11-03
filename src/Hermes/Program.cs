using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hermes.Data.DataAccess;
using Hermes.IoC;
using Hermes.QuartzScheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hermes
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Hermes service is up!!");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Hermes service fail to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    Configure(hostContext);

                    services.AddHostedService<QuartzHostedService>();
                    services.AddDbContext<HermesContext>(
                        options => options.UseNpgsql(Configuration.GetConnectionString("LoggerDb"),
                            npgsqlOptions => npgsqlOptions.UseNodaTime()));
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new QuartzModule(Configuration));
                    builder.RegisterModule(new LoggerModule());
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(Configuration)
                        .CreateLogger();

                    loggingBuilder.AddSerilog(Log.Logger, dispose: true);
                });

        private static void Configure(HostBuilderContext hostContext)
        {
            var env = hostContext.HostingEnvironment.EnvironmentName;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}