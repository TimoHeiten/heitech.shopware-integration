using System.Threading.Tasks;
using heitech.ShopwareIntegration.Models;
using ShopwareIntegration.Requests;

namespace heitech.ShopwareIntegration.ProductUseCases
{
    public sealed class WriteUseCase
    {
        private readonly ShopwareClient _client;
        private readonly ReadUseCases _read;
        private readonly WritingRequest<Product> _writer;

        public WriteUseCase(ShopwareClient client)
        {
            _client = client;
            _read = new ReadUseCases(_client);
            _writer = _client.CreateWriter<Product>();
        }

        public async Task<bool> InsertAsync(object product)
        {
            try
            {
                var result = await _writer.Create(product);
                if (!result.IsSuccess)
                    System.Console.WriteLine(result.Exception);
                return result.IsSuccess;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> Update(string productId, int increaseStockBy = 0, decimal? newPrice = null)
        {
            var product = await _read.GetProductById(productId, expandManufacturer: false);
            try
            {
                var result = await _writer.Update(
                    product.Id, 
                    new 
                    { 
                        stock = product.Stock + increaseStockBy, 
                        availableStock = product.AvailableStock + increaseStockBy,
                        price = new 
                        {
                            gross = newPrice ?? product.Price[0].Gross
                        }
                    }
                );
                return result.IsSuccess ? true : throw result.Exception;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
        }
    }
}