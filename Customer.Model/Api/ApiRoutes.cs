namespace Customer.Model.Api
{
    /// <summary>
    /// Defines the API route constants for customer service endpoints.
    /// </summary>
    public static class ApiRoutes
    {
        public const string ApiBase = "/api";

        public static class Health
        {
            public const string Live = "/health/live";
            public const string Ready = "/health/ready";
        }
    }
}
