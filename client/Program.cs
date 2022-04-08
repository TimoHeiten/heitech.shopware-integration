using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ShopwareIntegration.Client;
using ShopwareIntegration.Models;
using ShopwareIntegration.Requests;

namespace client
{
    // todo
    // shopware backend -> User Administration/Settings --> Edit User && mark enabled checkbox at API Access
    //      returns API Key which si requried for authentication
    // --> " If the edited user is currently logged in, you might need to clear the backend cache, and then log out and log in for your changes to take effect."
    // ----------------------------------------------------------------------
    // Test Auth 
    // alle Entitäten als library
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = await ShopwareClient.CreateAsync();

            ShopwareRequest<Address> addressRequest = null!;
            var key = args.FirstOrDefault();
            if (key is not null && key == "get")
                addressRequest = GetRequest(client);
            else
                addressRequest = PutRequest();

            /* System.Console.WriteLine(
                addressRequest.GetRequest(new System.Uri("https://localhost/api/"))
                              .Content
                              .ReadAsStringAsync()
                              .Result
                );
            */

            var result = await client.SendRequestAsync<Address>(addressRequest, CancellationToken.None);

            if (result.IsSuccess)
                System.Console.WriteLine($"Success!! - {result.Model}");
            else
                System.Console.WriteLine($"FAILED!! - {result.Exception}");
        }

        static ShopwareRequest<Address> PutRequest()
            => new Address(42, 1, "salutation", "firstName", "lastName", "street", "zipCode", "city", 1).CreatePutRequest();

        static ShopwareRequest<Address> GetRequest(ShopwareClient client)
            => client.CreateGetRequest<Address, int>(42);
    }
}
