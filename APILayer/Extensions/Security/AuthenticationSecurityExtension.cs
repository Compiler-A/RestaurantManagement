using BusinessLayerRestaurant.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APILayer.Extensions.Security
{
    public static class AuthenticationSecurityExtension
    {
        
        public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services , IConfiguration Configuration)
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // TokenValidationParameters define how incoming JWTs will be validated.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettingsSection.Issuer,
                    ValidAudience = jwtSettingsSection.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey))
                };
            });
            return services;
        }
    }
}
