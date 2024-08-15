using System.Net.Http.Headers;

namespace SimpleNewTab.Api.Bing
{
    public sealed class BingService : IImageMetadataService
    {
        private readonly HttpClient _HttpClient;
        private readonly BingConfiguration _Configuration;
        private readonly IValidator<ImageDto> _Validator;
        private readonly IMapper<ImageDto, ImageMetadata> _Mapper;

        public BingService(
            HttpClient httpClient,
            BingConfiguration configuration,
            IValidator<ImageDto> validator,
            IMapper<ImageDto, ImageMetadata> mapper)
        {
            _HttpClient = httpClient;
            _Configuration = configuration;
            _Validator = validator;
            _Mapper = mapper;
        }

        public async Task<ImageMetadata> GetLatest(CancellationToken cancellationToken)
        {
            var url = $"{_Configuration.BaseUrl}{_Configuration.LatestImageRoute}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.UserAgent.Add(Metadata.ProductInfo);
            var comment = new ProductInfoHeaderValue(_Configuration.UserAgentComment);
            request.Headers.UserAgent.Add(comment);
            using var response = await _HttpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var imageArchiveDto = await response.Content.ReadFromJsonAsync<ImageArchiveDto>(cancellationToken);
            var imageDto = imageArchiveDto?.Images?.Single();
            ArgumentNullException.ThrowIfNull(imageDto);
            _Validator.ValidateAndThrow(imageDto);
            var imageMetadata = _Mapper.Map(imageDto);

            return imageMetadata;
        }
    }
}
