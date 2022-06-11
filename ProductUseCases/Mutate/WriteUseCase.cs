using System.Linq;
using System.Text.Json;
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

        public Task<bool> InsertAsync()
        {
            return Task.FromResult(true);
        }

        public async Task<bool> Update(string productId, int increaseStockBy = 0, decimal? newPrice = null)
        {
            var product = await _read.GetProductById(productId, false);
            product.AvailableStock += increaseStockBy;
            product.Stock += increaseStockBy;
            product.Price[0].Gross = newPrice ?? product.Price[0].Gross;

            try
            {
                var result = await _writer.Update(product.Id, new { stock = product.Stock + increaseStockBy });
                return result.IsSuccess;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
        }
    }
}