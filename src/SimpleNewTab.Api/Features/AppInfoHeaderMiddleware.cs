namespace SimpleNewTab.Api.Features
{
    public sealed class AppInfoHeaderMiddleware
    {
        private readonly RequestDelegate _Next;

        public AppInfoHeaderMiddleware(RequestDelegate next)
        {
            _Next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Append(Metadata.AppInfoHeaderName, Metadata.AppInfo);

            return _Next(context);
        }
    }
}
