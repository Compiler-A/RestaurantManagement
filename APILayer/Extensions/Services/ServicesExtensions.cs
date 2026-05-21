using APILayer.Authorization.Employee;
using APILayer.Authorization.Order;
using ContractsLayerRestaurant.Configuration;
using ContractsLayerRestaurant.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace APILayer.Extensions.Services
{
    public static partial class ServicesExtensions
    {
        public static IServiceCollection AddServicesExtensions(this IServiceCollection Services, IConfiguration Configuration)
        {
            string? DataAccessStrategy = Configuration["DataType:DataAccessStrategy"];

            if (DataAccessStrategy == null)
            {
                throw new InvalidOperationException("Strategy is Null!");
            }

            Services.AddLoginServices();
            Services.AddEmployeesServices(DataAccessStrategy);
            Services.AddJobRolesServices(DataAccessStrategy);
            Services.AddMenuItemsServices(DataAccessStrategy);
            Services.AddOrderDetailsServices(DataAccessStrategy);
            Services.AddOrdersServices(DataAccessStrategy);
            Services.AddSettingsServices(DataAccessStrategy);
            Services.AddStatusMenusServices(DataAccessStrategy);
            Services.AddStatusOrdersServices(DataAccessStrategy);
            Services.AddStatusTablesServices(DataAccessStrategy);
            Services.AddTablesServices(DataAccessStrategy);
            Services.AddTypeItemsServices(DataAccessStrategy);
            Services.AddHashingServices();
            Services.AddSingleton<IAuthorizationHandler, EmployeeUserNameOwnerOrAdminHandler>();
            Services.AddSingleton<IAuthorizationHandler, EmployeeOwnerOrAdminHandler>();
            Services.AddSingleton<IAuthorizationHandler, WaiterOwnerOrAdminHandler>();
            Services.AddSingleton<IMyLogger, clsMyLogger>();
            return Services;
        }
    }
}
