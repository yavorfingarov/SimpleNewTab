namespace SimpleNewTab.Api.Data
{
    public sealed class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("Default"), dbOptions =>
                {
                    dbOptions.MigrationsHistoryTable("_Migration");
                });
            });
        }
    }
}
