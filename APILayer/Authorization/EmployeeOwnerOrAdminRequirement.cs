using Microsoft.AspNetCore.Authorization;

namespace APILayer.Authorization
{
    public class EmployeeOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
