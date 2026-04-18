using Microsoft.AspNetCore.Authorization;

namespace APILayer.Authorization
{
    public class EmployeeUserNameOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
