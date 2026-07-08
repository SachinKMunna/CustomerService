using Customer.Model.Api.V1.Request;
using Customer.Model.Api.V1.Response;
using Customer.WebApi.DataStore;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerDataProvider _customerDataProvider;

    public CustomerService(ICustomerDataProvider customerDataProvider)
        => _customerDataProvider = customerDataProvider;

    public async Task<CustomerResponse> RegisterAsync(
        RegisterCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = new DomainModel.Customer
        {
            Email = request.Email?.ToLowerInvariant(),
            Name = request.Name!,
            Phone = request.Phone!
        };

        var created = await _customerDataProvider.CreateAsync(customer, cancellationToken);

        return MapToResponse(created);
    }

    public async Task<RegisterCustomersResponse> RegisterBulkAsync(
        RegisterCustomersRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customers = request.Customers!
            .Select(req => new DomainModel.Customer
            {
                Email = req.Email?.ToLowerInvariant(),
                Name = req.Name!,
                Phone = req.Phone!
            })
            .ToList();

        var created = await _customerDataProvider.CreateBulkAsync(customers, cancellationToken);

        var responses = created.Select(MapToResponse).ToList();

        return new RegisterCustomersResponse
        {
            Customers = responses,
            TotalRegistered = responses.Count
        };
    }

    private static CustomerResponse MapToResponse(DomainModel.Customer customer) =>
        new()
        {
            Id = customer.Id,
            Email = customer.Email,
            Name = customer.Name,
            Phone = customer.Phone,
            CreatedAt = customer.CreatedAt
        };
}
