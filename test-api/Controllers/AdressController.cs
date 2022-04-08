using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopwareIntegration.Models;
using ShopwareIntegration.Models.Filters;

namespace test_api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromBody] FilterObject? filter)
        {
            var address = new Address { Id = id, Company = "42" };
            System.Console.WriteLine($"'{filter?.ToString()}' - filter");
            System.Console.WriteLine(address.Id + " " + address.Customer + $" {address.FirstName} {address.LastName}");
            return Ok(address);
        }
    }
}
