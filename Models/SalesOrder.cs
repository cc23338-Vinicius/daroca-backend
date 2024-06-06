//  SalesOrder?
//     OrderId : int
//     CustomerId : int Req
//     OrderDate : DateTime Req
//     EstimatedDeliveryDate : date
//     Status : string(20) req

public class SalesOrder {
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public required DateTime OrderDate { get; set; }
    public required DateOnly EstimatedDeliveryDate { get; set; }
    public required string Status { get; set; }

    public virtual required Customer Customer { get; set; }
    public virtual ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
}