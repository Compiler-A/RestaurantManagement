namespace APILayer.Extensions.Security
{
    public static partial class SecurityExtension
    {
        public static IServiceCollection AddCorsSecurityExtension(this IServiceCollection services, IConfiguration Configuration)
        {
            
            services.AddCors(options =>
            {
                options.AddPolicy("RMApiCorsPolicy", policy =>
                {
                    policy.WithOrigins(
                        Configuration["CORS:Web1"]!,
                        Configuration["CORS:Web2"]!
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
