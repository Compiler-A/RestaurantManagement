using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APILayer.Extensions.Security
{
    public static class AuthenticationSecurityExtension
    {
        public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services)
        {
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
                    ValidIssuer = "RMAPI",
                    ValidAudience = "RMAPIEmployees",
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_RM123456"))
                };
            });
            return services;
        }
    }
}
