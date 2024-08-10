namespace SimpleNewTab.Api.Bing
{
    public sealed class ImageArchiveDtoMapper : IMapper<ImageArchiveDto, ImageMetadata>
    {
        private readonly BingConfiguration _Configuration;

        public ImageArchiveDtoMapper(BingConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public ImageMetadata Map(ImageArchiveDto source)
        {
            var image = source.Images.Single();
            var dailyImageMetadata = new ImageMetadata()
            {
                Url = $"{_Configuration.BaseUrl}{image.Url}",
                Title = image.Title,
                QuizUrl = $"{_Configuration.BaseUrl}{image.Quiz}",
                Copyright = image.Copyright,
                CopyrightUrl = image.CopyrightLink,
            };

            return dailyImageMetadata;
        }

        public ImageArchiveDto Map(ImageMetadata source) => throw new NotImplementedException();
        public void Map(ImageArchiveDto source, ImageMetadata destination) => throw new NotImplementedException();
        public void Map(ImageMetadata source, ImageArchiveDto destination) => throw new NotImplementedException();
    }
}
