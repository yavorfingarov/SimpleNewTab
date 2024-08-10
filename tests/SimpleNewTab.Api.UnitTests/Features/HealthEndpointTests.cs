namespace SimpleNewTab.Api.UnitTests.Features
{
    public class HealthEndpointTests : TestBase
    {
        [Fact]
        public Task Handle()
        {
            Hydrate(ImageMetadata("test"));

            var result = HealthEndpoint.Handle(DataContext);

            return Verify(result);
        }

        [Fact]
        public Task Handle_DbEmpty()
        {
            var result = HealthEndpoint.Handle(DataContext);

            return Verify(result);
        }
    }
}
