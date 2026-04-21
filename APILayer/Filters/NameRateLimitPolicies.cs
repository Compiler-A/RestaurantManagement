namespace APILayer.Filters
{
    public static class NameRateLimitPolicies
    {
        public const string Auth = "AuthLimiter";
        public const string GetAll = "GetAllLimiter";
        public const string GetOne = "GetOneLimiter";
        public const string Add = "AddLimiter";
        public const string Update = "UpdateLimiter";
        public const string Delete = "DeleteLimiter";
    }
}
