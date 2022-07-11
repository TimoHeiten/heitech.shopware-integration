# heitech.shopware-integration
A Library to integrate the shopware6 Rest Api into your .NET C# Code.

To install the package of the StateManager (see below, recommended way to interact).
simply use:
```bash
dotnet add package heitech.ShopwareIntegration.State
```
[or see for yourself on Nuget Repository](https://www.nuget.org/packages/heitech.ShopwareIntegration.State)

## References
Check the [API reference here](https://developer.shopware.com/docs/resources/references/api-reference). 

Also see this Site for handling of [Authentication](https://shopware.stoplight.io/docs/admin-api/ZG9jOjEwODA3NjQx-authentication)


## Idea
This library allows you to access <b>a wrapped HttpClient</b> that handles all the Authentication and Shopware API specific quirks.

You can use the lower level ShopwareClient instance which is just a thin wrapper including a retry for the Authentication.

<b>But I would recommend the IStateManager interface</b>, which can be accessed via the <i>heitech.ShopwareIntegration.State.Factory</i> static class.
This also includes a cache, a logger and a set of higher level methods that handle all the setup for the REST API like filters, associations and more.

You will need a specific DataContext for each Type of request (Get, GetAll, Create, Update, Delete) but it is easier to use, since you do not need to know all the Shopware API quirks. Except for the filter objects, that are either supplied via the IFIlter Interface, its implementations or the FilterBuilder class. But the easiest way is to create an anonymous object (see example below. )

Here you can find the [Filtering reference from the API](https://shopware.stoplight.io/docs/store-api/ZG9jOjEwODExNzU2-search-queries).

## Example:
A simple generic call to get a Page of the Entity of Type T (BaseEntity)
```csharp
public async Task<IEnumerable<T>> GetAsync<T>(int pageNo, object? includes = null)
        where T : BaseEntity
{
    // create an Instance of the IStateManager with the default pipeline
    // including the logger and custom memory cache
    IStateManager state = await Factory.CreateAsync(
        new HttpClientConfiguration() 
        /* set the configuration to your shop/demo-shop to test it properly*/
    );

    // creates an anonymous object that will be serialized as the filterObject 
    // for the api endpoint
    var filter = new
    {
        limit = 100,
        page = pageNo,
        includes = includes
    };
    // the DataContext object that specifies a Paged request
    var pageContext = DataContext.GetPage<T>(pageNo: filter.page);
    // apply the filter for the request Pipeline
    pageContext.SetFilter(filter);

    var details = (await state.RetrievePage<T>(pageContext)).ToArray();

    return details;
}

```

## Models
Some Models do exist already, but for the ones you might need, you have to create your own Models

Therefore:
1. Inherit from BaseEntity. (allows for common json serialization/deserialization methods and properties)
2. Add the ModelUriAttribute. (this appendeds the supplied string to the BaseAddress of the wrapped HttpClient: '/api/{modelUri}').
    the modelURI can be found in the [overview](https://shopware.stoplight.io/docs/admin-api/ZG9jOjE0MzUyOTMz-entity-reference)

You don´t need to specify all Properties, only the ones you want to access, since the BaseEntity has a Property called AdditionalProperties. Also you can specify via the Includes filter (see heitech.ShopwareIntegration.ProductUseCases.IncludesFields class) so only the needed fields are fetched and deserialized. (to save on bandwidth)


example of Unit Ressource from the [Shopware Api](https://shopware.stoplight.io/docs/admin-api/c2NoOjE0MzUxMzUz-unit).
```csharp
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using heitech.ShopwareIntegration.Configuration;

namespace heitech.ShopwareIntegration.Models
{
    [ModelUri("unit")]
    public sealed class Unit : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("shortCode")]
        public string ShortCode { get; set; } = default!;

        [JsonPropertyName("translated")]
        public bool Translated { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("customFields")]
        public object CustomFields { get; set; } = default!;

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();

        public Unit() : base()
        { }
    }
}

```
