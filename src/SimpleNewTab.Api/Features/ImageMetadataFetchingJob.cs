using Polly.Registry;
using Polly.Retry;

namespace SimpleNewTab.Api.Features
{
    [Cron($"15 7 * * *")]
    public sealed class ImageMetadataFetchingJob : IJob
    {
        private readonly ILogger<ImageMetadataFetchingJob> _Logger;
        private readonly DataContext _DataContext;
        private readonly IImageMetadataService _ImageMetadataService;
        private readonly TimeProvider _TimeProvider;
        private readonly ResiliencePipelineProvider<string> _ResiliencePipelineProvider;

        public ImageMetadataFetchingJob(
            ILogger<ImageMetadataFetchingJob> logger,
            DataContext dataContext,
            IImageMetadataService imageMetadataService,
            TimeProvider timeProvider,
            ResiliencePipelineProvider<string> resiliencePipelineProvider)
        {
            _Logger = logger;
            _DataContext = dataContext;
            _ImageMetadataService = imageMetadataService;
            _TimeProvider = timeProvider;
            _ResiliencePipelineProvider = resiliencePipelineProvider;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            var resiliencePipeline = _ResiliencePipelineProvider.GetPipeline(nameof(ImageMetadataFetchingJob));
            var latestImageMetadata = await resiliencePipeline.ExecuteAsync(GetLatestImageMetadata, cancellationToken);
            if (latestImageMetadata == null)
            {
                _Logger.LogError("Could not fetch latest image metadata.");

                return;
            }

            _DataContext.ImageMetadata.Add(latestImageMetadata);
            _DataContext.SaveChanges();
            _Logger.LogInformation("Successfully fetched latest image metadata.");
        }

        private async ValueTask<ImageMetadata?> GetLatestImageMetadata(CancellationToken cancellationToken)
        {
            var latestImageMetadata = await _ImageMetadataService.GetLatest(cancellationToken);
            var latestDbImageMetadataUrl = _DataContext.ImageMetadata
                .AsNoTracking()
                .OrderBy(x => x.Expiration)
                .Select(x => x.Url)
                .LastOrDefault();
            if (latestImageMetadata.Url == latestDbImageMetadataUrl)
            {
                return null;
            }

            var now = _TimeProvider.GetUtcNow();
            latestImageMetadata.Expiration = now.AddDays(1);

            return latestImageMetadata;
        }

        public static void ConfigureResilience(ResiliencePipelineBuilder builder)
        {
            builder.AddRetry(new RetryStrategyOptions()
            {
                Delay = TimeSpan.FromHours(1),
                MaxRetryAttempts = 5,
                ShouldHandle = new PredicateBuilder()
                    .HandleResult(x => x == null)
            });
        }
    }
}
