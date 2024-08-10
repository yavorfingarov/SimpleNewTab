namespace SimpleNewTab.Api.Common
{
    public sealed class ImageMetadata
    {
        public DateTimeOffset Expiration { get; set; }
        public string Url { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string QuizUrl { get; set; } = null!;
        public string Copyright { get; set; } = null!;
        public string CopyrightUrl { get; set; } = null!;
    }
}
