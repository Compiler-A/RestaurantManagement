using APILayer.Filters;
using ContractsLayerRestaurant.DTORequest.Auth;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace APILayer.Extensions.Security
{
    

    public static class RateLimitingSecurityExtension
    {

        private static void _AddPolicies(RateLimiterOptions options,RateLimitePoliciesOptions RLP)
        {
            options.AddPolicy(RLP.NamePolicy, httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ip,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = RLP.PermitLimit,
                        Window = TimeSpan.FromMinutes(RLP.TimeSpam),
                        QueueLimit = RLP.QueueLimit
                    });
            });
        }

        public static WebApplication UseRateLimitingExtension(this WebApplication app)
        {
            app.UseRateLimiter();
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                }
            });
            return app;
        }

        public static IServiceCollection AddRateLimitingExtension(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                _AddPolicies(options,new RateLimitePoliciesOptions(NameRateLimitPolicies.Auth, 5, 1, 0));
                _AddPolicies(options,new RateLimitePoliciesOptions(NameRateLimitPolicies.GetAll, 30, 1, 0));
                _AddPolicies(options,new RateLimitePoliciesOptions(NameRateLimitPolicies.GetOne, 60, 1, 0));
                _AddPolicies(options,new RateLimitePoliciesOptions(NameRateLimitPolicies.Add, 10, 1, 0));
                _AddPolicies(options,new RateLimitePoliciesOptions(NameRateLimitPolicies.Update, 15, 1, 0));
                _AddPolicies(options,new RateLimitePoliciesOptions(NameRateLimitPolicies.Delete, 5, 1, 0));
            });
            return services;
        }
    }
}
