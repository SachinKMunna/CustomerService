namespace Customer.Model.Api.V1.Response;

/// <summary>
/// Response from bulk customer registration endpoint.
/// Contains list of successfully registered customers and total count.
/// </summary>
public class RegisterCustomersResponse
{
    /// <summary>
    /// List of successfully registered customers.
    /// </summary>
    public required List<CustomerResponse> Customers { get; set; }

    /// <summary>
    /// Total number of customers registered in this bulk operation.
    /// </summary>
    public int TotalRegistered { get; set; }
}
