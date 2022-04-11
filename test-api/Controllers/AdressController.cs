using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopwareIntegration.Models;

namespace test_api.Controllers
{
    // dummy api for testing the models etc.
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromBody] IEnumerable<object>? filter)
            => Ok(new Address { Id = id, Company = "42" });

        [HttpPut]
        public IActionResult Put([FromBody] Address? address)
            =>  Ok(address);
    }
}
