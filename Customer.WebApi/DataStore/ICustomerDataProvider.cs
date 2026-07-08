using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.DataStore;

public interface ICustomerDataProvider
{
    Task<DomainModel.Customer> CreateAsync(DomainModel.Customer customer, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainModel.Customer>> CreateBulkAsync(IEnumerable<DomainModel.Customer> customers, CancellationToken cancellationToken = default);
    Task<DomainModel.Customer?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<DomainModel.Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
