using Customer.Model.Api.V1.Cart;
using Customer.WebApi.Domain.Security;
using Customer.WebApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customer.WebApi.Controllers.V1
{
    /// <summary>
    /// Shopping cart management for authenticated customers.
    /// </summary>
    [Route("api/v1/cart/{email}")]
    public sealed class CartController : ControllerWithAuthorization
    {
        private readonly ICartService _cartService;
        private readonly ICurrentCustomer _currentCustomer;

        public CartController(ICartService cartService, ICurrentCustomer currentCustomer)
        {
            _cartService = cartService;
            _currentCustomer = currentCustomer;
        }

        /// <summary>
        /// Add an item to the customer's cart.
        /// </summary>
        /// <remarks>
        /// Only authenticated customers can add items to their own cart.
        /// If customer does not have a cart, one will be created automatically.
        /// </remarks>
        /// <param name="email">The customer email</param>
        /// <param name="request">The item to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns 201 Created with updated cart</returns>
        /// <response code="201">Item successfully added to cart</response>
        /// <response code="400">Invalid request - missing required fields</response>
        /// <response code="403">Forbidden - cannot modify another customer's cart</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("items")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddItem(
            [FromRoute] string email,
            [FromBody] AddCartItemRequest request,
            CancellationToken cancellationToken)
        {
            if (!IsAuthorizedCustomer(email))
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = await _cartService.AddItemAsync(email, request, cancellationToken);
            return Created(string.Empty, cart);
        }

        /// <summary>
        /// Remove an item from the customer's cart.
        /// </summary>
        /// <remarks>
        /// Only authenticated customers can remove items from their own cart.
        /// </remarks>
        /// <param name="email">The customer email</param>
        /// <param name="productId">The product ID to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns 200 OK with updated cart</returns>
        /// <response code="200">Item successfully removed from cart</response>
        /// <response code="403">Forbidden - cannot modify another customer's cart</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("items/{productId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveItem(
            [FromRoute] string email,
            [FromRoute] string productId,
            CancellationToken cancellationToken)
        {
            if (!IsAuthorizedCustomer(email))
                return Forbid();

            var cart = await _cartService.RemoveItemAsync(email, productId, cancellationToken);
            return Ok(cart);
        }

        private bool IsAuthorizedCustomer(string email)
            => _currentCustomer.IsAuthenticated && _currentCustomer.Email == email;
    }
}
