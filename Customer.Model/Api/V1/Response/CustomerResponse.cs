namespace Customer.Model.Api.V1.Response;

public class CustomerResponse
{
    public required string Id { get; set; }
    public string? Email { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}
