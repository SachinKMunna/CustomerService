namespace Customer.Model.Api.V1.Cart;

public class CartResponse
{
    public required string CartId { get; set; }
    public required string Email { get; set; }
    public required List<CartItemResponse> Items { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
