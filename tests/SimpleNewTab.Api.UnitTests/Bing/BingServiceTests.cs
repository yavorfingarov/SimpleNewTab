using System.Net;
using System.Net.Http.Json;
using SimpleNewTab.Api.Bing;
using VerifyTests.Http;

namespace SimpleNewTab.Api.UnitTests.Bing
{
    public class BingServiceTests : IDisposable
    {
        private HttpResponseMessage? _HttpResponseMessage;
        private readonly MockHttpClient _HttpClient;
        private readonly BingService _BingService;

        public BingServiceTests()
        {
            var configuration = new BingConfiguration()
            {
                BaseUrl = "https://www.bing.com",
                LatestImageRoute = "/latest-image",
                UserAgentComment = "(+http://example.com)"
            };

            _HttpClient = new MockHttpClient(_ => _HttpResponseMessage!);
            _BingService = new BingService(
                _HttpClient,
                configuration,
                new ImageArchiveDtoValidator(),
                new ImageArchiveDtoMapper(configuration));
        }

        [Fact]
        public async Task GetLatest()
        {
            SetResponse(HttpStatusCode.OK, ImageArchiveDto());

            var imageMetadata = await _BingService.GetLatest(CancellationToken.None);

            await Verify(new { imageMetadata, _HttpClient.Calls });
        }

        [Fact]
        public async Task GetLatest_NotFound()
        {
            SetResponse(HttpStatusCode.NotFound, null);

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_NoBody()
        {
            SetResponse(HttpStatusCode.OK, null);

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_ImagesNull()
        {
            SetResponse(HttpStatusCode.OK, new ImageArchiveDto(null!));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_NoImages()
        {
            var images = Enumerable.Empty<ImageDto>();
            SetResponse(HttpStatusCode.OK, new ImageArchiveDto(images));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_TwoImages()
        {
            var images = Enumerable.Repeat(ImageDto(), 2);
            SetResponse(HttpStatusCode.OK, new ImageArchiveDto(images));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_InvalidUrl()
        {
            SetResponse(HttpStatusCode.OK, ImageArchiveDto(url: " "));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_InvalidTitle()
        {
            SetResponse(HttpStatusCode.OK, ImageArchiveDto(title: " "));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_InvalidQuizUrl()
        {
            SetResponse(HttpStatusCode.OK, ImageArchiveDto(quiz: " "));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_InvalidCopyright()
        {
            SetResponse(HttpStatusCode.OK, ImageArchiveDto(copyright: " "));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GetLatest_InvalidCopyrightUrl()
        {
            SetResponse(HttpStatusCode.OK, ImageArchiveDto(copyrightLink: " "));

            await ThrowsTask(() => _BingService.GetLatest(CancellationToken.None))
                .IgnoreStackTrace();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _HttpResponseMessage?.Dispose();
            _HttpClient.Dispose();
        }

        private void SetResponse(HttpStatusCode statusCode, ImageArchiveDto? imageArchiveDto)
        {
            _HttpResponseMessage = new HttpResponseMessage(statusCode)
            {
                Content = JsonContent.Create(imageArchiveDto)
            };
        }

        private static ImageDto ImageDto(
            string? url = null,
            string? title = null,
            string? quiz = null,
            string? copyright = null,
            string? copyrightLink = null)
        {
            var imageDto = new ImageDto(
                url ?? "/image.jpg",
                title ?? "Test title",
                quiz ?? "/quiz",
                copyright ?? "Some author",
                copyrightLink ?? "https://some-author.com");

            return imageDto;
        }

        private static ImageArchiveDto ImageArchiveDto(
            string? url = null,
            string? title = null,
            string? quiz = null,
            string? copyright = null,
            string? copyrightLink = null)
        {
            var imageArchiveDto = new ImageArchiveDto(new[]
            {
                ImageDto(url, title, quiz, copyright, copyrightLink)
            });

            return imageArchiveDto;
        }
    }
}
