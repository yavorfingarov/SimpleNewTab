using System.Diagnostics;

namespace SimpleNewTab.Api.Features
{
    [Endpoint]
    public sealed class ImageMetadataEndpoint
    {
        [Get("/api/image-metadata/latest")]
        public static ImageMetadata Handle(DataContext dataContext, TimeProvider timeProvider)
        {
            var now = timeProvider.GetUtcNow();
            var imageMetadata = dataContext.ImageMetadata
                .AsNoTracking()
                .Where(x => x.Expiration > now)
                .FirstOrDefault();
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
                imageMetadata.Expiration = now.AddHours(2);

                return imageMetadata;
            }

            throw new UnreachableException("Could not get random image metadata.");
        }
    }
}
