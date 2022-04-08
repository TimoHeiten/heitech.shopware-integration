using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using models;
using models.filters;

namespace test_api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AdressController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromBody] FilterObject? filter)
        {
            var address = new Address(id, 42, 
                "conmpany", "department", "salutation", "firstName",
                "lastName", "street", "zipCode", "phone", "vatId", "", "", 42, 42, Array.Empty<object>()
            );
            System.Console.WriteLine(filter?.ToString());
            System.Console.WriteLine(address.Id + " " + address.Customer + $" {address.FirstName} {address.LastName}");
            return Ok(address);
        }
    }
}
