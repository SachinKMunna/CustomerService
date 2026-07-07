namespace Customer.Tests.Utilities
{
    /// <summary>
    /// Fixed JWT values shared between the test host and the token factory so that
    /// tokens minted in tests validate against the running app.
    /// </summary>
    public static class TestAuth
    {
        public const string Issuer = "https://test.customer.local";
        public const string Audience = "customer-api";
        public const string Key = "test-signing-key-that-is-long-enough-for-hs256-0123456789";
    }
}
