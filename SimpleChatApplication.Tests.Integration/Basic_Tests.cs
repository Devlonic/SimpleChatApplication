using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SimpleChatApplication.Tests.Integration {
    public class Basic_Tests : IClassFixture<WebApplicationFactory<Program>> {
        private readonly WebApplicationFactory<Program> factory;

        public Basic_Tests(WebApplicationFactory<Program> factory) {
            this.factory = factory;
        }

        [Theory]
        [InlineData("api/User/SignIn")]
        public async Task Post_ReturnsSuccessfullCodes(string url) {
            var client = factory.CreateClient();

            var json = JsonSerializer.Serialize(new {
                UserName = "Username123"
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var responce = await client.PostAsync(url, content);
            responce.EnsureSuccessStatusCode();
            Assert.True(true);
        }
    }
}
