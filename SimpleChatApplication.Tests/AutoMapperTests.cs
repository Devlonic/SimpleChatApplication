using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.BLL.Mappings;
using SimpleChatApplication.DAL.Data.Contexts;
using System.Reflection;
using Xunit;
namespace SimpleChatApplication.Tests {
    public class AutoMapperTests {
        [Fact]
        public async Task Should_BeValidConfiguration() {
            var autoMapperConfiguration = new MapperConfiguration(config => {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(ChatApplicationDbContext).Assembly));
            });

            autoMapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
