using APILayer.Authorization.Employee;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace APILayer.Authorization.Order
{
    public class WaiterOwnerOrAdminHandler
        : AuthorizationHandler<WaiterOwnerOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context, WaiterOwnerOrAdminRequirement requirement, int EmployeeID)
        {
            if (context.User.IsInRole("Manager") || context.User.IsInRole("Chef") || context.User.IsInRole("Sous Chef"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedEmployeeId) &&
                authenticatedEmployeeId == EmployeeID)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
