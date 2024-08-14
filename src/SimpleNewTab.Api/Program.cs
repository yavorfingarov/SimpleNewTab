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
            logger.Info("Application starting. Environment: {Environment} / AppInfo: {AppInfo}",
                builder.Environment.EnvironmentName, Metadata.AppInfo);

            try
            {
                Run(builder);
                logger.Info("Application stopping gracefully.");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Application could not start.");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static void Run(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();

            builder.Host.UseNLog();

            builder.ConfigureServices();

            var app = builder.Build();

            app.UseExceptionHandling();

            app.UseMiddleware<AppInfoHeaderMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors();

            app.MapEndpoints();

            app.Run();
        }
    }
}
