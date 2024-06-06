public class SalesOrderItem { 
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }

    public virtual required SalesOrder SalesOrder { get; set; }
    public virtual required Product Product { get; set; }
}