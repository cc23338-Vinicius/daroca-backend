public class Stock{
    public int StockId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public virtual required Product Product { get; set; }
}
