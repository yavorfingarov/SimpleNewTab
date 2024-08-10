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
        public static string AppInfo { get; }
        public static ProductInfoHeaderValue ProductInfo { get; }

        static Metadata()
        {
            var assembly = typeof(Metadata).Assembly;
            var assemblyName = assembly.GetName()?.Name;
            var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            ArgumentNullException.ThrowIfNull(assemblyName);
            ArgumentNullException.ThrowIfNull(informationalVersion);
            var informationalVersionTokens = informationalVersion.InformationalVersion.Split("-");
            var build = informationalVersionTokens.ElementAtOrDefault(1) ?? "dev";
            AppInfo = $"{assemblyName}/{build}";
            ProductInfo = new ProductInfoHeaderValue(assemblyName, build);
        }
    }
}
