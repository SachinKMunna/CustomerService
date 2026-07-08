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

        /// <summary>
        /// Register a single new customer.
        /// </summary>
        /// <remarks>
        /// Creates a new customer account with the provided email, name, and phone information.
        /// Email is optional and will be normalized to lowercase for consistency.
        /// </remarks>
        /// <param name="request">Customer registration request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns 201 Created with the registered customer details</returns>
        /// <response code="201">Customer successfully registered</response>
        /// <response code="400">Invalid request - missing required fields (Name or Phone)</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Register multiple customers in bulk (at once).
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to create many customer accounts simultaneously in a single API call.
        /// Useful for batch imports, migrations, or administrative bulk customer onboarding.
        /// 
        /// All customers in the request are validated before insertion. If any customer record is invalid,
        /// the entire bulk operation is rejected with a 400 Bad Request error.
        /// 
        /// Minimum requirement: At least 1 customer must be provided.
        /// Each customer requires: Name and Phone (Email is optional).
        /// </remarks>
        /// <param name="request">Bulk registration request containing a list of customers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns 201 Created with list of registered customers and total count</returns>
        /// <response code="201">Successfully registered all customers</response>
        /// <response code="400">Invalid request - missing required fields or empty customer list</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("bulk")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RegisterCustomersResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterBulk(
            [FromBody] RegisterCustomersRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customersToCreate = request.Customers!
                .Select(req => new DomainModel.Customer
                {
                    Email = req.Email?.ToLowerInvariant(),
                    Name = req.Name!,
                    Phone = req.Phone!
                })
                .ToList();

            var createdCustomers = await _customerDataProvider.CreateBulkAsync(customersToCreate, cancellationToken);

            var responses = createdCustomers
                .Select(c => new CustomerResponse
                {
                    Id = c.Id,
                    Email = c.Email,
                    Name = c.Name,
                    Phone = c.Phone,
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            var bulkResponse = new RegisterCustomersResponse
            {
                Customers = responses,
                TotalRegistered = responses.Count
            };

            return CreatedAtAction(nameof(RegisterBulk), bulkResponse);
        }
    }
}
