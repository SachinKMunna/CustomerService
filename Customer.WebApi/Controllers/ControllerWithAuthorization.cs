using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.WebApi.Controllers
{
    /// <summary>
    /// Base controller that requires an authenticated caller for all actions.
    /// </summary>
    [ApiController]
    [Authorize]
    public abstract class ControllerWithAuthorization : ControllerBase
    {
    }
}
