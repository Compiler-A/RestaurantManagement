using ContractsLayerRestaurant.Configuration;

namespace APILayer.Extensions.Configuration
{
    public static partial class ConfigurationExtension
    {
        public static IServiceCollection AddJwtSettingConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            var secretKey = Configuration["JWT_SECRET_KEY"];
            var jwtSettingsSection = Configuration.GetSection("Jwt").Get<JwtSettings>();
            if (jwtSettingsSection == null)
            {
                throw new InvalidOperationException(
                    "jwt Setting Section is Null");
            }

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException(
                    "JWT_SECRET_KEY is missing or empty in environment variables.");
            }
            Services.Configure<JwtSettings>(options =>
            {
                options.SecretKey = secretKey;
                options.Issuer = jwtSettingsSection.Issuer;
                options.Audience = jwtSettingsSection.Audience;
                options.ExpirationMinutes = jwtSettingsSection.ExpirationMinutes;
            });
            return Services;
        }
    }
}
