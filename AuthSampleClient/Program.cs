using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthSampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var client = new HttpClient();

            // discover endpoints from the metadata by calling Auth server hosted on 5000 port
            var discoveryClient = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (discoveryClient.IsError)
            {
                Console.WriteLine(discoveryClient.Error);
                return;
            }

            // request the token from the Auth server
            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = discoveryClient.TokenEndpoint,
                ClientId = "sitecoreclient",
                ClientSecret = "secret",
                GrantType= "client_credentials",

                Parameters =
                {
                    {
                        "scope","customAPI.write"
                    }
                }
            });

            if (response.IsError)
            {
                Console.WriteLine(response.Error);
                return;
            }
            Console.WriteLine(response.Json);
        }
    }
}
