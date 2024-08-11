using SimpleNewTab.Api.Bing;

namespace SimpleNewTab.Api.Features
{
    public sealed class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddResiliencePipeline(nameof(ImageMetadataFetchingJob), ImageMetadataFetchingJob.ConfigureResilience);

            builder.Services.AddHttpClient<IImageMetadataService, BingService>()
                .AddStandardResilienceHandler();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithMethods("GET");
                    policy.AllowAnyOrigin();
                });
            });
        }
    }
}
