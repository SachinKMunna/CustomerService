namespace CustomerService.Model.Api
{
    /// <summary>
    /// Well-known route and documentation constants shared across the API surface.
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
