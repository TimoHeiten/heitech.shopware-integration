using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using heitech.ShopwareIntegration.Core.Configuration;
using Xunit;

namespace heitech.ShopwareIntegration.Core.Tests
{
    public sealed class ConfigurationTests
    {
        HttpClientConfiguration Expected => new HttpClientConfiguration("http://localhost:4300/shopware/api/", "clientId", "userName", "clientSecret");

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
    }
}