using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShopwareIntegration.Client;

namespace client
{
    // todo
    // shopware backend -> User Administration/Settings --> Edit User && mark enabled checkbox at API Access
    //      returns API Key which si requried for authentication
    // --> " If the edited user is currently logged in, you might need to clear the backend cache, and then log out and log in for your changes to take effect."
    // ----------------------------------------------------------------------
    // alle Entitäten als library
    // Test Auth 
    // sende an meine eigene API
    // 
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = await ShopwareClient.CreateAsync();
            var request = client.CreateGetRequest<ShopwareIntegration.Models.Address, int>(1);
            var result = await client.SendRequestAsync<ShopwareIntegration.Models.Address>(request, CancellationToken.None);

            if (result.IsSuccess)
                System.Console.WriteLine($"Success!! - {result.Model}");
            else
                System.Console.WriteLine($"FAILED!! - {result.Exception}");
        }
    }
}
