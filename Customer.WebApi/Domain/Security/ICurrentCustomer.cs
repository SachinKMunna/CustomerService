namespace Customer.WebApi.Domain.Security
{
    /// <summary>
    /// Represents the currently authenticated customer, resolved from the incoming JWT.
    /// Consumed by domain services and controllers; implemented by an infrastructure adapter.
    /// </summary>
    public interface ICurrentCustomer
    {
        bool IsAuthenticated { get; }

        /// <summary>External identity id from the token (the "sub" claim).</summary>
        string? ExternalAuthId { get; }

        string? Email { get; }
    }
}
