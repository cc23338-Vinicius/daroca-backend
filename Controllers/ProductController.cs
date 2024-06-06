using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("/[controller]")]
[ApiController]
public class ProductController : ControllerBase {
    private readonly DatabaseContext context;

    public ProductController(DatabaseContext context) {
        this.context = context;
    }   

    [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts() {
        return this.context.Product.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id) {
        var Product = this.context.Product.Find(id);
        if(Product == null) {
            return NotFound();
        }

        return Product;
    }

    [HttpPost]
    public ActionResult<Product> CreateProduct(Product Product) {
        if (Product == null) {
            return BadRequest("null");
        }
        this.context.Product.Add(Product);
        this.context.SaveChanges();

        return CreatedAtAction(nameof(GetProduct), new {id = Product.ProductId}, Product);
    }

    [HttpDelete("{id}")]
    public ActionResult<Product> DeleteProduct(int id) {
        var Product = this.context.Product.Find(id);
        if(Product == null) {
            return NotFound();
        }

        this.context.Product.Remove(Product);
        this.context.SaveChanges();

        return NoContent();
        //return Ok(Product);
    }

}
