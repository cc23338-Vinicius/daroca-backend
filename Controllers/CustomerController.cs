using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("/[controller]")]
[ApiController]
public class CustomerController : ControllerBase {
    private readonly DatabaseContext context;

    public CustomerController(DatabaseContext context) {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers() {
        return this.context.Customer.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Customer> GetCustomer(int id) {
        var customer = this.context.Customer.Find(id);
        if(customer == null) {
            return NotFound();
        }

        return customer;
    }

    [HttpPost]
    public ActionResult<Customer> CreateCustomer(Customer customer) {
        if (customer == null) {
            return BadRequest("null");
        }
        this.context.Customer.Add(customer);
        this.context.SaveChanges();

        return CreatedAtAction(nameof(GetCustomer), new {id = customer.CustomerId}, customer);
    }

    [HttpDelete("{id}")]
    public ActionResult<Customer> DeleteCustomer(int id) {
        var customer = this.context.Customer.Find(id);
        if(customer == null) {
            return NotFound();
        }

        this.context.Customer.Remove(customer);
        this.context.SaveChanges();

        return NoContent();
        //return Ok(customer);
    }

}
