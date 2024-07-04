using Microsoft.AspNetCore.Mvc; // To use ControllerBase, [Route], [ApiController], and so on.
using Northwind.WebApi.Repositories; // To use ICustomerRepository, CustomerRepository.
using Northwind.EntityModels; // To use Customer.
namespace Northwind.WebApi.Controllers
{
    //base address: api/customer
    [ApiController]
    [Route("/api/[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repo;
        public CustomerController(ICustomerRepository repo)
        {
            _repo = repo;
        }


        //GET: api/customers
        //GET: api/customers/?country=[country]
        //This will always return a list customers but the list might be empty.
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public async Task<IEnumerable<Customer>> GetCustomers([FromQuery] string? country)
        {
            if (String.IsNullOrWhiteSpace(country))
            {
                return await _repo.RetrieveAllAsync();
            }
            else
            {
                return (await _repo.RetrieveAllAsync()).Where(c => c.Country == country);
            }
        }

        //GET: api/customers/{id}
        [HttpGet("{id}", Name = nameof(GetCustomer))]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCustomer(string id)
        {
            Customer c = await _repo.RetrieveAsync(id);
            if (c == null)
            {
                return NotFound();
            }
            return Ok(c);
        }

        //POST: api/customers
        //BODY: Customer (JSON, XML)
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            Customer? addedCustomer = await _repo.CreateAsync(customer);

            if (addedCustomer == null)
            {
                return BadRequest("Repository failed to create customer.");
            }


            return CreatedAtRoute(routeName: nameof(GetCustomer), routeValues: new { id = addedCustomer.CustomerId.ToLower() }, value: addedCustomer);
        }

        //PUT: api/customers/[id]
        //BODY: Customer (JSON, XML)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, [FromBody] Customer c)
        {
            id = id.ToUpper();
            c.CustomerId = c.CustomerId.ToUpper();

            if (id is null || c.CustomerId != id)
            {
                return BadRequest();// 400 BadRequest
            }
            Customer? existing = await _repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound(); //  404 NotFound
            }
            await _repo.UpdateAsync(c);
            return new NoContentResult(); // 204 no content
        }

        //Delete: api/customer/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == "bad")
            {
                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://localhost:5151/customers/failed-to-delete",
                    Title = $"Customer ID {id} found but failed to delete.",
                    Detail = "More details like Company Name, Country and so on.",
                    Instance = HttpContext.Request.Path
                };
                return BadRequest(problemDetails); // 400 Bad Request
            }

            Customer? existing = await _repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound(); // 404 notfound
            }
            bool? deleted = await _repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult();// 204
            }
            else
            {
                return BadRequest($"Customer {id} was found but failed to delete."); // 400 BadRequest.
            }
        }
    }
}
