using Customer.Model.Api.V1.Request;
using Customer.Model.Api.V1.Response;

namespace Customer.WebApi.Domain.Services;

public interface ICustomerService
{
    Task<CustomerResponse> RegisterAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default);
    Task<RegisterCustomersResponse> RegisterBulkAsync(RegisterCustomersRequest request, CancellationToken cancellationToken = default);
}
