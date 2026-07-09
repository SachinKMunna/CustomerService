using Customer.Model.Api.V1.Request;
using Customer.Model.Api.V1.Response;
using Customer.WebApi.DataStore;
using Customer.WebApi.Infrastructure.Mappers;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerDataProvider _customerDataProvider;
    private readonly IMappingCoordinator _mappingCoordinator;

    public CustomerService(ICustomerDataProvider customerDataProvider, IMappingCoordinator mappingCoordinator)
    {
        _customerDataProvider = customerDataProvider;
        _mappingCoordinator = mappingCoordinator;
    }

    public async Task<CustomerResponse> RegisterAsync(
        RegisterCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = _mappingCoordinator.Map<RegisterCustomerRequest, DomainModel.Customer>(request);

        var created = await _customerDataProvider.CreateAsync(customer, cancellationToken);

        return _mappingCoordinator.Map<DomainModel.Customer, CustomerResponse>(created);
    }

    public async Task<RegisterCustomersResponse> RegisterBulkAsync(
        RegisterCustomersRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customers = _mappingCoordinator.Map<RegisterCustomerRequest, DomainModel.Customer>(request.Customers!).ToList();

        var created = await _customerDataProvider.CreateBulkAsync(customers, cancellationToken);

        var responses = _mappingCoordinator.Map<DomainModel.Customer, CustomerResponse>(created).ToList();

        return new RegisterCustomersResponse
        {
            Customers = responses,
            TotalRegistered = responses.Count
        };
    }
}
