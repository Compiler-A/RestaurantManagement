using APILayer.Authorization.Employee;
using APILayer.Authorization.Order;
using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace APILayer.Extensions.Services
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServicesExtensions(this IServiceCollection Services)
        {

            Services.AddLoginServices();
            Services.AddEmployeesServices();
            Services.AddJobRolesServices();
            Services.AddMenuItemsServices();
            Services.AddOrderDetailsServices();
            Services.AddOrdersServices();
            Services.AddSettingsServices();
            Services.AddStatusMenusServices();
            Services.AddStatusOrdersServices();
            Services.AddStatusTablesServices();
            Services.AddTablesServices();
            Services.AddTypeItemsServices();
            Services.AddHashingServices();
            Services.AddSingleton<IAuthorizationHandler, EmployeeUserNameOwnerOrAdminHandler>();
            Services.AddSingleton<IAuthorizationHandler, EmployeeOwnerOrAdminHandler>();
            Services.AddSingleton<IAuthorizationHandler, WaiterOwnerOrAdminHandler>();
            Services.AddSingleton<IMyLogger, clsMyLogger>();
            return Services;
        }
    }
}
