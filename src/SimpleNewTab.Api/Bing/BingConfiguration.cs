namespace SimpleNewTab.Api.Bing
{
    [Configuration("Bing")]
    public sealed class BingConfiguration
    {
        public string BaseUrl { get; set; } = null!;
        public string LatestImageRoute { get; set; } = null!;
        public string UserAgentComment { get; set; } = null!;
    }
}
