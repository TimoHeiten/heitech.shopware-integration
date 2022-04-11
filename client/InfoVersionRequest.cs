using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.Digest;
using ShopwareIntegration.Configuration;

namespace client
{
    internal class InfoVersionRequest
    {
        public static async Task RunAsync()
        {
            var configuration = await HttpClientConfiguration.LoadAsync().ConfigureAwait(false);
            if (configuration is null)
                throw new NullReferenceException($"The HttpConfiguration could not be loaded at: {typeof(InfoVersionRequest)} {nameof(RunAsync)}");

            RestClient client = new(configuration.BaseUrl) { Authenticator = GetAuthenticator(configuration) };

            var request = new RestRequest("_info/version", Method.Get);
            // request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = await client.ExecuteAsync(request);

            if (response.ErrorException is null)
            {
                var content = response.Content;
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("HTTP Request for _info/version at shopware API Failed");
                Console.WriteLine(response.ErrorException.Message);
            }
        }

        private static IAuthenticator GetAuthenticator(HttpClientConfiguration configuration)
        {
            return new HttpBasicAuthenticator(configuration.UserName, configuration.Password);
            return new DigestAuthenticator(configuration.UserName, configuration.Password);
        }
    }
}