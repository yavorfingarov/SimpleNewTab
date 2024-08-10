using System.Net.Http.Headers;

namespace SimpleNewTab.Api.Bing
{
    public sealed class BingService : IImageMetadataService
    {
        private readonly HttpClient _HttpClient;
        private readonly BingConfiguration _Configuration;
        private readonly IValidator<ImageArchiveDto> _Validator;
        private readonly IMapper<ImageArchiveDto, ImageMetadata> _Mapper;

        public BingService(
            HttpClient httpClient,
            BingConfiguration configuration,
            IValidator<ImageArchiveDto> validator,
            IMapper<ImageArchiveDto, ImageMetadata> mapper)
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
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(_Configuration.UserAgentComment));
            using var response = await _HttpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var imageArchiveDto = await response.Content.ReadFromJsonAsync<ImageArchiveDto>(cancellationToken);
            ArgumentNullException.ThrowIfNull(imageArchiveDto);
            await _Validator.ValidateAndThrowAsync(imageArchiveDto, cancellationToken);
            var imageMetadata = _Mapper.Map(imageArchiveDto);

            return imageMetadata;
        }
    }
}
