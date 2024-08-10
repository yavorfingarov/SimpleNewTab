using NLog;
using NLog.Config;
using NLog.Web;

namespace SimpleNewTab.Api
{
    public static class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();
            LogManager.Configuration.Install(new InstallationContext());
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            try
            {
                Run(builder);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Application could not start.");
            }

            LogManager.Shutdown();
        }

        private static void Run(WebApplicationBuilder builder)
        {
            builder.ConfigureServices();

            var app = builder.Build();

            app.Logger.LogInformation("Application starting. Environment: {Environment}", app.Environment.EnvironmentName);

            app.UseExceptionHandling();

            app.UseMiddleware<AppInfoHeaderMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors();

            app.MapEndpoints();

            app.Run();
        }
    }
}
