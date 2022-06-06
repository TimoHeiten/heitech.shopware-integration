﻿using System;
using System.Threading.Tasks;
using ShopwareIntegration;
using ShopwareIntegration.Requests;
using ShopwareIntegration.Models;
using heitech.ShopwareIntegration.Requests;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // test store creds
            var configuration = new ShopwareIntegration.Configuration.HttpClientConfiguration(
                baseUrl: "your shopware api",
                clientId: "your client id",
                userName: string.Empty,
                clientSecret: "your client secret"
            );
            var client = await ShopwareClient.CreateAsync(configuration).ConfigureAwait(false);

            await CreateUnit(client);
            return;

            // examples. Remove the return statement to check
            var liste = new ReadRequest<CustomerGroup>(client);
            await ExecuteUpdate(client);
            return;
            _ = await liste.ExecuteGetAsync("713d0f385267496e9629564d314c1ec4");
            System.Console.WriteLine("next");
            System.Console.WriteLine();
            System.Console.ReadLine();

            // associations
            var inner = new { limit = 3 };
            var ass = new { customers = inner };
            _ = await liste.SearchAsync(new { associations = ass });
            _ = await liste.SearchIdsAsync(new { associations = ass });
        }

        static async Task ExecuteRequestDemo(ShopwareClient client, string uri)
        {
            var result = await client.GetAsync<Product>(uri).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                var container = result.Model;
                System.Console.WriteLine($"Success!! - {container}");
            }
            else
                System.Console.WriteLine($"FAILED!! - {result.Exception}");
        }

        static async Task ExecuteUpdate(ShopwareClient client)
        {
            // get first by Id 
            var read = new ReadRequest<CustomerGroup>(client);
            var item = await read.ExecuteGetAsync("713d0f385267496e9629564d314c1ec4");
            var obj = item.Model.Data;

            var write = new WritingRequest<CustomerGroup>(client);
            obj.Name = "updated with Patch";
            var writeResult = await write.Update(obj);
            var s = writeResult.IsSuccess ? $"{writeResult.Model}" : "failed";
            System.Console.WriteLine(s);

            var item2 = await read.ExecuteGetAsync("713d0f385267496e9629564d314c1ec4");
            var obj2 = item.Model.Data;
            System.Console.WriteLine();
            System.Console.WriteLine(obj2.Name);
            System.Console.WriteLine();
        }

        static async Task CreateUnit(ShopwareClient client)
        {
            var unit = new Unit
            {
                Name = "unit-1",
                ShortCode = "shortCode",
                Translated = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CustomFields = null,
                Products = new()
            };

            var write = new WritingRequest<Unit>(client);
            var rqResult = await write.Create(unit);
            System.Console.WriteLine("Create is Success: " + rqResult.IsSuccess);
            if (!rqResult.IsSuccess) return;

            unit.Name = "Updated";
            rqResult = await write.Update(unit);
            System.Console.WriteLine("Update is Success: " + rqResult.IsSuccess);
            if (!rqResult.IsSuccess) return;

            rqResult = await write.Delete(unit);
            System.Console.WriteLine("Delete is Success: " + rqResult.IsSuccess);
        }
    }
}
