global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using NSubstitute;
global using SimpleNewTab.Api.Common;
global using SimpleNewTab.Api.Data;
global using SimpleNewTab.Api.Features;
global using VerifyTests.MicrosoftLogging;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test names can contain underscores.")]

namespace SimpleNewTab.Api.UnitTests
{
    public class ModuleInit
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyHttp.Initialize();
            VerifyMicrosoftLogging.Initialize();
            VerifyEntityFramework.Initialize();
        }
    }
}
