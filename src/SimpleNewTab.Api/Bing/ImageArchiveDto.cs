namespace SimpleNewTab.Api.Bing
{
    public sealed record ImageArchiveDto(IEnumerable<ImageDto> Images);

    public sealed record ImageDto(
        string Url,
        string Title,
        string Quiz,
        string Copyright,
        string CopyrightLink);
}
