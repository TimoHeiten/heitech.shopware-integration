using System.Linq;
using System.Threading.Tasks;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.Models;
using ShopwareIntegration.Requests;

namespace heitech.ShopwareIntegration.ProductUseCases
{
    public sealed class ReadUseCases
    {
        private readonly ShopwareClient _client;
        private readonly ReadRequest<Product> _reader;
        private readonly ReadRequest<ProductManufacturer> _associationReader;
        public ReadUseCases(ShopwareClient client)
        {
            _client = client;
            _reader = client.CreateReader<Product>();
            _associationReader = client.CreateReader<ProductManufacturer>();
        }


        ///<summary>
        /// Get a page of Products from the Api. You need to specify the actual page.
        /// With the Includes Parameter you can specify the required fields on the products.
        ///</summary>
        public async Task<ProductsResult> GetProductsPage(ProductPaging? page = null, object? includes = null)
        {
            const int firstPage = 1;

            var currentPage = page ?? new ProductPaging(firstPage);
            var filter = new
            {
                limit = currentPage.Amount,
                page = currentPage.Page,
                includes = includes ?? new IncludesFields.Product("name", "price", "id", "active", "availableStock", "description", "ean", "stock", "manufacturerId"),
            };

            var productsResult = await _reader.SearchAsync(filter.FromAnonymous());

            return productsResult.IsSuccess
                   ? new(productsResult.Model.Data, currentPage)
                   : throw productsResult.Exception;
        }

        ///<summary>
        /// Get Product By Id. Usually you want to only supply the Id. Specify the QueryString or Filter for a more customized approach
        ///</summary>
        public async Task<Product> GetProductById(ProductByIdParameter productById, bool expandManufacturer = true)
        {
            if (productById.Filter is null)
            {
                var productResult = await _reader.ExecuteGetAsync(productById.Id, productById.Query);

                if (productResult.IsSuccess)
                {
                    var product = productResult.Model!.Data;
                    if (expandManufacturer)
                    {
                        var manufacturerResult = await _associationReader.ExecuteGetAsync(productResult.Model?.Data?.ManufacturerId ?? "");
                        product.Manufacturer = manufacturerResult.Model?.Data;
                    }

                    return product;
                }

                throw productResult.Exception;
            }

            var searchResult = await _reader.SearchAsync(productById.Filter);

            return searchResult.IsSuccess
                   ? searchResult.Model.Data.First()
                   : throw searchResult.Exception;
        }
    }
}