global using FluentValidation;
global using Microsoft.EntityFrameworkCore;
global using Polly;
global using Reprise;
global using SimpleNewTab.Api.Common;
global using SimpleNewTab.Api.Data;
global using SimpleNewTab.Api.Features;

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Logging performance is not an issue.")]

namespace SimpleNewTab.Api
{
    public static class Metadata
    {
        public static string AppInfoHeaderName => "X-App-Info";
        public static ProductInfoHeaderValue ProductInfo { get; }
        public static string AppInfo { get; }

        static Metadata()
        {
            var assembly = typeof(Metadata).Assembly;
            var assemblyName = assembly.GetName();
            var appName = assemblyName.Name;
            var version = assemblyName.Version;
            ArgumentNullException.ThrowIfNull(appName);
            ArgumentNullException.ThrowIfNull(version);
            var versionString = $"{version.Major:00}.{version.Minor:00}.{version.Build:00}.{version.Revision:0000}";
            ProductInfo = new ProductInfoHeaderValue(appName, versionString);
            AppInfo = ProductInfo.ToString();
        }
    }
}
