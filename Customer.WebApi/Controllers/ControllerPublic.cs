using Microsoft.AspNetCore.Mvc;

namespace Customer.WebApi.Controllers
{
    /// <summary>
    /// Base controller for public endpoints that do not require authentication.
    /// </summary>
    [ApiController]
    public abstract class ControllerPublic : ControllerBase
    {
    }
}
