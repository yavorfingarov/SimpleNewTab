using System.Diagnostics.CodeAnalysis;

namespace SimpleNewTab.Api.Bing
{
    public sealed class ImageDtoMapper : IMapper<ImageDto, ImageMetadata>
    {
        private readonly BingConfiguration _Configuration;

        public ImageDtoMapper(BingConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public ImageMetadata Map(ImageDto source)
        {
            var imageMetadata = new ImageMetadata()
            {
                Url = $"{_Configuration.BaseUrl}{source.Url}",
                Title = source.Title,
                QuizUrl = $"{_Configuration.BaseUrl}{source.Quiz}",
                Copyright = source.Copyright,
                CopyrightUrl = source.CopyrightLink,
            };

            return imageMetadata;
        }

        [ExcludeFromCodeCoverage]
        public ImageDto Map(ImageMetadata source) => throw new NotImplementedException();

        [ExcludeFromCodeCoverage]
        public void Map(ImageDto source, ImageMetadata destination) => throw new NotImplementedException();

        [ExcludeFromCodeCoverage]
        public void Map(ImageMetadata source, ImageDto destination) => throw new NotImplementedException();
    }
}
