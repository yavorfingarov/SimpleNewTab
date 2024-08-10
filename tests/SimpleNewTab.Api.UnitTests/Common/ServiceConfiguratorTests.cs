using Microsoft.AspNetCore.Builder;
using Reprise;

namespace SimpleNewTab.Api.UnitTests.Common
{
    public sealed class ServiceConfiguratorTests
    {
        [Theory]
        [MemberData(nameof(GetServiceConfiguratorTypes))]
        public Task ServiceConfigurator(Type type)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder();
            webApplicationBuilder.Services.Clear();
            var configurator = (IServiceConfigurator)Activator.CreateInstance(type)!;

            configurator.ConfigureServices(webApplicationBuilder);

            return Verify(webApplicationBuilder.Services)
                .IgnoreMembersThatThrow<InvalidOperationException>()
                .UseParameters(type.Namespace!.Replace("SimpleNewTab.Api.", ""));
        }

        public static TheoryData<Type> GetServiceConfiguratorTypes()
        {

            var serviceConfiguratorTypes = typeof(Program).Assembly.GetExportedTypes()
                .Where(t => t.IsAssignableTo(typeof(IServiceConfigurator)))
                .ToList();
            var theoryData = new TheoryData<Type>(serviceConfiguratorTypes);

            return theoryData;
        }
    }
}
