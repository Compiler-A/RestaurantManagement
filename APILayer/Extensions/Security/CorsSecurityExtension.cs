namespace APILayer.Extensions.Security
{
    public static class CorsSecurityExtension
    {
        public static IServiceCollection AddCorsSecurityExtension(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("RMApiCorsPolicy", policy =>
                {
                    policy.WithOrigins(
                        "https://localhost:7292",
                        "http://localhost:5223"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
