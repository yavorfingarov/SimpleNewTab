using System.Diagnostics;

namespace SimpleNewTab.Api.Features
{
    [Endpoint]
    public sealed class ImageMetadataEndpoint
    {
        private static readonly TimeSpan _RandomImageMetadataExpiration = TimeSpan.FromHours(2);

        [Get("/api/image-metadata/latest")]
        public static ImageMetadata Handle(DataContext dataContext, TimeProvider timeProvider)
        {
            var now = timeProvider.GetUtcNow();
            var imageMetadata = dataContext.ImageMetadata
                .AsNoTracking()
                .Where(x => x.Expiration > now)
                .OrderBy(x => x.Expiration)
                .LastOrDefault();
            if (imageMetadata != null)
            {
                return imageMetadata;
            }

            imageMetadata = dataContext.ImageMetadata
                .AsNoTracking()
                .OrderBy(x => EF.Functions.Random())
                .FirstOrDefault();
            if (imageMetadata != null)
            {
                imageMetadata.Expiration = now.Add(_RandomImageMetadataExpiration);

                return imageMetadata;
            }

            throw new UnreachableException("Could not get random image metadata.");
        }
    }
}
