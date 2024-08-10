namespace SimpleNewTab.Api.Common
{
    public interface IImageMetadataService
    {
        Task<ImageMetadata> GetLatest(CancellationToken cancellationToken);
    }
}
