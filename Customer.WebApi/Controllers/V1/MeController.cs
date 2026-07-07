using Customer.WebApi.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace Customer.WebApi.Controllers.V1
{
    /// <summary>
    /// Returns the authenticated caller's identity. Used to verify JWT authentication
    /// end to end (PR-Auth). Superseded by richer profile endpoints in later PRs.
    /// </summary>
    [Route("api/v1/me")]
    public sealed class MeController : ControllerWithAuthorization
    {
        private readonly ICurrentCustomer _currentCustomer;

        public MeController(ICurrentCustomer currentCustomer)
            => _currentCustomer = currentCustomer;

        [HttpGet]
        public IActionResult Get()
            => Ok(new
            {
                isAuthenticated = _currentCustomer.IsAuthenticated,
                externalAuthId = _currentCustomer.ExternalAuthId,
                email = _currentCustomer.Email
            });
    }
}
