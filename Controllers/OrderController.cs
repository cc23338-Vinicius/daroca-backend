using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("/[controller]")]
[ApiController]
public class OrderController : ControllerBase {
    private readonly DatabaseContext context;

    public OrderController(DatabaseContext context) {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SalesOrder>> GetOrders() {
        return this.context.SalesOrder.Include(o => o.Items).ThenInclude(i => i.Product).ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<SalesOrder> GetOrder(int id) {
        var order = this.context.SalesOrder.Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefault(o => o.OrderId == id);
        if (order == null) {
            return NotFound();
        }

        return order;
    }

    [HttpPost]
    public ActionResult<SalesOrder> CreateOrder(SalesOrder order) {
        if (order == null) {
            return BadRequest("null");
        }

        foreach (var item in order.Items) {
            var stock = context.Stock.FirstOrDefault(s => s.ProductId == item.ProductId);
            if (stock == null || stock.Quantity < item.Quantity) {
                return BadRequest("Estoque insuficiente para o produto com ID " + item.ProductId);
            }
            stock.Quantity -= item.Quantity;
            context.Stock.Update(stock);
        }

        this.context.SalesOrder.Add(order);
        this.context.SaveChanges();

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public ActionResult<SalesOrder> UpdateOrder(int id, SalesOrder order) {
        if (id != order.OrderId) {
            return BadRequest();
        }

        var existingOrder = this.context.SalesOrder.Include(o => o.Items).FirstOrDefault(o => o.OrderId == id);
        if (existingOrder == null) {
            return NotFound();
        }

        // Update order logic here

        this.context.SalesOrder.Update(existingOrder);
        this.context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<SalesOrder> DeleteOrder(int id) {
        var order = this.context.SalesOrder.Include(o => o.Items).FirstOrDefault(o => o.OrderId == id);
        if (order == null) {
            return NotFound();
        }

        // Revert stock quantities
        foreach (var item in order.Items) {
            var stock = context.Stock.FirstOrDefault(s => s.ProductId == item.ProductId);
            if (stock != null) {
                stock.Quantity += item.Quantity;
                context.Stock.Update(stock);
            }
        }

        this.context.SalesOrder.Remove(order);
        this.context.SaveChanges();

        return NoContent();
    }
}
