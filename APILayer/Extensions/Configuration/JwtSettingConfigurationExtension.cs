using BusinessLayerRestaurant.Classes;

namespace APILayer.Extensions.Configuration
{
    public static class JwtSettingConfigurationExtension
    {
        public static IServiceCollection AddJwtSettingConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            var secretKey = Configuration["JWT_SECRET_KEY"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException(
                    "JWT_SECRET_KEY is missing or empty in environment variables.");
            }
            Services.Configure<JwtSettings>(options =>
            {
                options.SecretKey = secretKey;
                options.Issuer = "RMAPI";
                options.Audience = "RMAPIEmployees";
                options.ExpirationMinutes = 10;
            });
            return Services;
        }
    }
}
