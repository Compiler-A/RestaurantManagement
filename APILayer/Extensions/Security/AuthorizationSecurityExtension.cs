using APILayer.Authorization.Employee;
using APILayer.Authorization.Order;

namespace APILayer.Extensions.Security
{
    public static partial class SecurityExtension
    {
        public static IServiceCollection AddAuthorizationExtension(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EmployeeOwnerOrAdmin", policy =>
                    policy.Requirements.Add(new EmployeeOwnerOrAdminRequirement()));
                options.AddPolicy("EmployeeByUserNameOwnerOrAdmin", policy =>
                    policy.Requirements.Add(new EmployeeUserNameOwnerOrAdminRequirement()));
                options.AddPolicy("WaiterOwnerOrAdmin", policy =>
                    policy.Requirements.Add(new WaiterOwnerOrAdminRequirement()));
            });

            return services;
        }
    }
}
