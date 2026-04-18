using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace APILayer.Authorization
{
    public class EmployeeUserNameOwnerOrAdminHandler
        : AuthorizationHandler<EmployeeUserNameOwnerOrAdminRequirement, string>
    {
        protected override Task HandleRequirementAsync(
           AuthorizationHandlerContext context,
           EmployeeUserNameOwnerOrAdminRequirement requirement,
           string userName)
        {
            if (context.User.IsInRole("Manager"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var username = context.User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(username))
                return Task.CompletedTask;

            if (username == userName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
