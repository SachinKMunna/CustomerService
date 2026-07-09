using System.ComponentModel.DataAnnotations;

namespace Customer.Model.Api.V1.Cart;

public class AddCartItemRequest
{
    [Required(ErrorMessage = "ProductId is required")]
    public string? ProductId { get; set; }

    [Required(ErrorMessage = "ProductName is required")]
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
}
