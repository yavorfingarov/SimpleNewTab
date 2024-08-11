namespace SimpleNewTab.Api.UnitTests.Features
{
    public class ImageMetadataEndpointTests : TestBase
    {
        [Fact]
        public Task Handle()
        {
            Hydrate(ImageMetadata("today", UtcNow.AddHours(3)));
            Hydrate(ImageMetadata("yesterday", UtcNow.AddDays(-1).AddHours(-3)));
            Recording.Start();

            var response = ImageMetadataEndpoint.Handle(DataContext, TimeProvider);

            return Verify(response)
                .DontScrubDateTimes();
        }

        [Fact]
        public Task Handle_NoFreshImageMetadata()
        {
            Hydrate(ImageMetadata("yesterday", UtcNow.AddDays(-1).AddHours(-3)));
            Recording.Start();

            var response = ImageMetadataEndpoint.Handle(DataContext, TimeProvider);

            return Verify(response)
                .DontScrubDateTimes();
        }

        [Fact]
        public Task Handle_NoImageMetadata()
        {
            Recording.Start();

            return Throws(() => ImageMetadataEndpoint.Handle(DataContext, TimeProvider))
                .IgnoreStackTrace();
        }
    }
}
