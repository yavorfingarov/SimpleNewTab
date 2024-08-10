using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleNewTab.Api.UnitTests.Features
{
    public class AppInfoHeaderMiddlewareTests
    {
        [Fact]
        public async Task Invoke()
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.UseTestServer();
                webHostBuilder.ConfigureServices(services =>
                {
                    services.AddRouting();
                });
                webHostBuilder.Configure(app =>
                {
                    app.UseMiddleware<AppInfoHeaderMiddleware>();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", () => Results.Ok());
                    });
                });
            });

            using var host = await hostBuilder.StartAsync();
            var client = host.GetTestClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var appInfo = response.Headers.GetValues(Metadata.AppInfoHeaderName).Single();
            Assert.Equal("SimpleNewTab.Api/dev", appInfo);
        }
    }
}
