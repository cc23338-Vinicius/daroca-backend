using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("/[controller]")]
[ApiController]
public class StockController : ControllerBase {
    private readonly DatabaseContext context;

    public StockController(DatabaseContext context) {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Stock>> GetStocks() {
        return this.context.Stock.Include(s => s.Product).ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Stock> GetStock(int id) {
        var stock = this.context.Stock.Include(s => s.Product).FirstOrDefault(s => s.StockId == id);
        if (stock == null) {
            return NotFound();
        }

        return stock;
    }

    [HttpPost]
    public ActionResult<Stock> CreateStock(Stock stock) {
        if (stock == null) {
            return BadRequest("null");
        }

        this.context.Stock.Add(stock);
        this.context.SaveChanges();

        return CreatedAtAction(nameof(GetStock), new { id = stock.StockId }, stock);
    }

    [HttpPut("{id}")]
        public ActionResult<Stock> UpdateStock(int id, Stock stock) {
            if (id != stock.StockId) {
                return BadRequest();
            }

            var existingStock = this.context.Stock.Find(id);
            if (existingStock == null) {
                return NotFound();
            }

        existingStock.ProductId = stock.ProductId;
        existingStock.Quantity = stock.Quantity;

        this.context.Stock.Update(existingStock);
        this.context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<Stock> DeleteStock(int id) {
        var stock = this.context.Stock.Find(id);
        if (stock == null) {
            return NotFound();
        }

        this.context.Stock.Remove(stock);
        this.context.SaveChanges();

        return NoContent();
    }
}
