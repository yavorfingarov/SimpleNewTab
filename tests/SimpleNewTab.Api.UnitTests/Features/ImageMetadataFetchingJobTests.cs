using NSubstitute;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Polly.Testing;

namespace SimpleNewTab.Api.UnitTests.Features
{
    public class ImageMetadataFetchingJobTests : TestBase
    {
        private readonly ImageMetadataFetchingJob _ImageMetadataFetchingJob;
        private readonly IImageMetadataService _ImageMetadataService;

        public ImageMetadataFetchingJobTests()
        {
            var loggerProvider = new RecordingProvider();
            var logger = loggerProvider.CreateLogger<ImageMetadataFetchingJob>();
            _ImageMetadataService = Substitute.For<IImageMetadataService>();
            var resiliencePipelineProvider = Substitute.For<ResiliencePipelineProvider<string>>();
            resiliencePipelineProvider.GetPipeline(nameof(ImageMetadataFetchingJob))
                .Returns(ResiliencePipeline.Empty);
            _ImageMetadataFetchingJob = new ImageMetadataFetchingJob(
                logger,
                DataContext,
                _ImageMetadataService,
                TimeProvider,
                resiliencePipelineProvider);
        }

        [Fact]
        public async Task Run_DbImageMetadataNotExpired()
        {
            Hydrate(ImageMetadata("today", UtcNow.AddHours(2)));
            Recording.Start();

            await _ImageMetadataFetchingJob.Run(CancellationToken.None);

            await Verify();
        }

        [Fact]
        public async Task Run_NoDbImageMetadata()
        {
            _ImageMetadataService.GetLatest(CancellationToken.None)
                .Returns(ImageMetadata("today"));
            Recording.Start();

            await _ImageMetadataFetchingJob.Run(CancellationToken.None);

            await Verify();
        }

        [Fact]
        public async Task Run_DbImageMetadataExpired()
        {
            Hydrate(ImageMetadata("yesterday", UtcNow));
            _ImageMetadataService.GetLatest(CancellationToken.None)
                .Returns(ImageMetadata("today"));
            Recording.Start();

            await _ImageMetadataFetchingJob.Run(CancellationToken.None);

            await Verify();
        }

        [Fact]
        public async Task Run_DbImageMetadataExpired_BeforeMidnight()
        {
            Hydrate(ImageMetadata("yesterday", UtcNow.AddDays(-1)));
            _ImageMetadataService.GetLatest(CancellationToken.None)
                .Returns(ImageMetadata("today"));
            TimeProvider.SetUtcNow(UtcNow.AddHours(12));
            Recording.Start();

            await _ImageMetadataFetchingJob.Run(CancellationToken.None);

            await Verify();
        }

        [Fact]
        public async Task Run_DbImageMetadataExpired_AlreadyFetched()
        {
            Hydrate(ImageMetadata("today", UtcNow.AddHours(-4)));
            _ImageMetadataService.GetLatest(CancellationToken.None)
                .Returns(ImageMetadata("today"));
            Recording.Start();

            await _ImageMetadataFetchingJob.Run(CancellationToken.None);

            await Verify();
        }

        [Fact]
        public async Task ConfigureResilience()
        {
            var resiliencePipelineBuilder = new ResiliencePipelineBuilder();

            ImageMetadataFetchingJob.ConfigureResilience(resiliencePipelineBuilder);

            var pipelineDescriptor = resiliencePipelineBuilder.Build().GetPipelineDescriptor();
            var strategy = pipelineDescriptor.Strategies.Single();
            var strategyOptions = (RetryStrategyOptions)strategy.Options!;
            var snapshot = await CreateSnapshot(strategyOptions);
            await Verify(snapshot)
                .IgnoreMembers("ShouldHandle", "Randomizer");
        }

        private static async Task<object> CreateSnapshot(RetryStrategyOptions strategyOptions)
        {
            var snapshot = new
            {
                Strategy = strategyOptions,
                ShouldHandleResults = new
                {
                    Null = await strategyOptions.ShouldHandle(OutcomeArgs(isNull: true)),
                    NotNull = await strategyOptions.ShouldHandle(OutcomeArgs(isNull: false))
                }
            };

            return snapshot;
        }

        private static RetryPredicateArguments<object> OutcomeArgs(bool isNull)
        {
            var outcome = isNull ? Outcome.FromResult<object>(default) : Outcome.FromResult<object>(new { });
            var args = new RetryPredicateArguments<object>(default!, outcome, default);

            return args;
        }
    }
}
