using Microsoft.AspNetCore.Authorization;

namespace APILayer.Authorization.Employee
{
    public class EmployeeUserNameOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
