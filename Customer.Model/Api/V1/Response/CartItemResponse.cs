namespace Customer.Model.Api.V1.Cart;

public class CartItemResponse
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; }
}
