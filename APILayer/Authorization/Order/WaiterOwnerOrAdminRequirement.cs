using Microsoft.AspNetCore.Authorization;

namespace APILayer.Authorization.Order
{
    public class WaiterOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
