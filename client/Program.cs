using System.Threading.Tasks;
using client.Commands;
using client.Options;
using CliFx;
using heitech.ShopwareIntegration.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace client
{
    public static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder().AddJsonFile("Options/settings.json")
                //.AddEnvironmentVariables("SW6_")
                .Build();

            services.Configure<ShopwareOptions>(config.GetSection("Config"));
            services.AddTransient<IShopwareClient>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<ShopwareOptions>>().Value;
                var task = ShopwareCoreFactory.CreateAsync(config.BaseUrl, config.ClientId, config.UserName, config.ClientSecret);

                return task.Result;
            });
            services.AddTransient<GetProductListWithFilter>();
            services.AddTransient<GetProductByIdCommand>();

            return await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(services.BuildServiceProvider())
                .Build()
                .RunAsync();
        }
    }
}