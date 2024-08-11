global using FluentValidation;
global using Microsoft.EntityFrameworkCore;
global using Polly;
global using Reprise;
global using SimpleNewTab.Api.Common;
global using SimpleNewTab.Api.Data;
global using SimpleNewTab.Api.Features;

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection;

[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Logging performance is not an issue.")]

namespace SimpleNewTab.Api
{
    public static class Metadata
    {
        public static string AppInfoHeaderName => "X-App-Info";
        public static ProductInfoHeaderValue ProductInfo { get; }
        public static string AppInfo { get; }
        public static string? Commit { get; }

        static Metadata()
        {
            var assembly = typeof(Metadata).Assembly;
            var assemblyName = assembly.GetName();
            var appName = assemblyName.Name;
            var version = assemblyName.Version;
            var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            ArgumentNullException.ThrowIfNull(appName);
            ArgumentNullException.ThrowIfNull(version);
            ArgumentNullException.ThrowIfNull(informationalVersion);
            var versionString = $"{version.Major:00}.{version.Minor:00}.{version.Build:00}.{version.Revision:0000}";
            var informationalVersionTokens = informationalVersion.InformationalVersion.Split("+");
            ProductInfo = new ProductInfoHeaderValue(appName, versionString);
            AppInfo = ProductInfo.ToString();
            Commit = informationalVersionTokens.ElementAtOrDefault(1);
        }
    }
}
