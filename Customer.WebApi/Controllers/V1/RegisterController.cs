using Customer.Model.Api.V1.Request;
using Customer.Model.Api.V1.Response;
using Customer.WebApi.DataStore;
using DomainModel = Customer.WebApi.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Customer.WebApi.Controllers.V1
{
    /// <summary>
    /// Register a new customer with email, name, and phone.
    /// </summary>
    [Route("api/v1/register")]
    public sealed class RegisterController : ControllerPublic
    {
        private readonly ICustomerDataProvider _customerDataProvider;

        public RegisterController(ICustomerDataProvider customerDataProvider)
            => _customerDataProvider = customerDataProvider;

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCustomerRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new DomainModel.Customer
            {
                Email = request.Email?.ToLowerInvariant(),
                Name = request.Name!,
                Phone = request.Phone!
            };

            var createdCustomer = await _customerDataProvider.CreateAsync(customer, cancellationToken);

            var response = new CustomerResponse
            {
                Id = createdCustomer.Id,
                Email = createdCustomer.Email,
                Name = createdCustomer.Name,
                Phone = createdCustomer.Phone,
                CreatedAt = createdCustomer.CreatedAt
            };

            return CreatedAtAction(nameof(Register), response);
        }
    }
}
