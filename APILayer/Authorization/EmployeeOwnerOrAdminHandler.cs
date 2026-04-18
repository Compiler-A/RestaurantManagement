using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace APILayer.Authorization
{
    public class EmployeeOwnerOrAdminHandler 
        : AuthorizationHandler<EmployeeOwnerOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync(
           AuthorizationHandlerContext context,
           EmployeeOwnerOrAdminRequirement requirement,
           int EmployeeID)
        {
            if (context.User.IsInRole("Manager"))
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
