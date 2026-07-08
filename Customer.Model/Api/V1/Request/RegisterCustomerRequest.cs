using System.ComponentModel.DataAnnotations;

namespace Customer.Model.Api.V1.Request;

public class RegisterCustomerRequest
{
    public string? Email { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    public string? Phone { get; set; }
}
