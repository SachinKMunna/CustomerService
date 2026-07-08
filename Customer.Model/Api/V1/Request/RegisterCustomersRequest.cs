using System.ComponentModel.DataAnnotations;

namespace Customer.Model.Api.V1.Request;

public class RegisterCustomersRequest
{
    [Required(ErrorMessage = "Customers list is required")]
    [MinLength(1, ErrorMessage = "At least one customer is required")]
    public List<RegisterCustomerRequest>? Customers { get; set; }
}
