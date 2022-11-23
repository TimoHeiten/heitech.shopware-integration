using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using heitech.ShopwareIntegration.Core.Configuration;
using heitech.ShopwareIntegration.Core.Data;
using Xunit;

namespace heitech.ShopwareIntegration.Core.Tests
{
    public sealed class ConfigurationTests
    {
        HttpClientConfiguration Expected => new("http://localhost:4300/shopware/api/",
            "clientId", "userName", "clientSecret");

        [Fact]
        public async Task LoadFromFile_Works()
        {
            // Arrange
            var stream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "httpconfig.json"));

            // Act
            var result = await HttpClientConfiguration.LoadAsync(stream);

            // Assert
            result.Should().BeEquivalentTo(Expected);
        }

        [Fact]
        public async Task LoadFrom_With_FileInfo_Works()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, "httpconfig.json"));

            // Act
            var result = await HttpClientConfiguration.LoadAsync(fileInfo);

            // Assert
            result.Should().BeEquivalentTo(Expected);
        }

        [Fact]
        public void Url_From_Type_Can_Be_found()
        {
            // Arrange
            const string expected = Authenticate.Url;

            // Act
            var url = ModelUri.GetUrlFromType<Authenticate>();

            // Assert
            url.Should().Be(expected);
        }

        [Fact]
        public void NoUrlAttribute_ThrowsArgumentNullException()
        {
            // Arrange
            // Act
            var act = ModelUri.GetUrlFromType<NoUrlAttribute>;

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class NoUrlAttribute
        {
        }
    }
}