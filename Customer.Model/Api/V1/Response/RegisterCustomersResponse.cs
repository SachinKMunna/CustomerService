namespace Customer.Model.Api.V1.Response;

public class RegisterCustomersResponse
{
    public required List<CustomerResponse> Customers { get; set; }
    public int TotalRegistered { get; set; }
}
