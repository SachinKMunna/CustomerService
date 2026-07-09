using System.ComponentModel.DataAnnotations;

namespace Customer.Model.Api.V1.Request;

/// <summary>
/// Request to register multiple customers at once (bulk registration).
/// </summary>
public class RegisterCustomersRequest
{
    /// <summary>
    /// List of customers to register in bulk.
    /// Minimum: 1 customer required. Each customer in the list must have Name and Phone.
    /// </summary>
    [Required(ErrorMessage = "Customers list is required")]
    [MinLength(1, ErrorMessage = "At least one customer is required")]
    public List<RegisterCustomerRequest>? Customers { get; set; }
}
