using System.Data;
using BusinessLayerRestaurant;
using DataLayerRestaurant;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<clsMySettings>(
    builder.Configuration.GetSection("MySettings"));

builder.Services.AddScoped<IReadableDEmployees, DataLayerRestaurant.clsEmployeesReader>();
builder.Services.AddScoped<IWritableDEmployees, DataLayerRestaurant.clsEmployeesWriter>();
builder.Services.AddScoped<IDataEmployees, clsDataEmployees>();
builder.Services.AddScoped<IReadableBEmployees, BusinessLayerRestaurant.clsEmployeesReader>();
builder.Services.AddScoped<IWritableBEmployees, BusinessLayerRestaurant.clsEmployeesWriter>();
builder.Services.AddScoped<IDTOBEmployees, clsEmployeesDtoContainer>();
builder.Services.AddScoped<IInterfaceBEmployees, clsEmployeesRepositoryBridge>();
builder.Services.AddScoped<ICompositionBEmployees, clsJobRoleLoader>();
builder.Services.AddScoped<IBusinessEmployees, clsBusinessEmployees>();

builder.Services.AddScoped<IReadableDJobRoles, DataLayerRestaurant.clsJobRolesReader>();
builder.Services.AddScoped<IWritableDJobRoles, DataLayerRestaurant.clsJobRolesWriter>();
builder.Services.AddScoped<IDataJobRoles, clsDataJobRoles>();
builder.Services.AddScoped<IWritableBJobRoles, BusinessLayerRestaurant.clsJobRolesWriter>();
builder.Services.AddScoped<IReadableBJobRoles, BusinessLayerRestaurant.clsJobRolesReader>();
builder.Services.AddScoped<IInterfaceBJobRoles, clsBJobRolesRepositoryBridge>();
builder.Services.AddScoped<IDTOBJobRoles, clsJobRolesDtoContainer>();
builder.Services.AddScoped<IBusinessJobRoles, clsBusinessJobRoles>();

builder.Services.AddScoped<IReadableDMenuItems, DataLayerRestaurant.clsMenuItemsReader>();
builder.Services.AddScoped<IWritableDMenuItems, DataLayerRestaurant.clsMenuItemsWriter>();
builder.Services.AddScoped<IDataMenuItems, clsDataMenuItems>();
builder.Services.AddScoped<ICompositionBMenuItems, clsTypeItemLoader>();
builder.Services.AddScoped<ICompositionBMenuItems, clsStatusMenuLoader>();
builder.Services.AddScoped<IDTOBMenuItems, clsMenuItemsDtoContainer>();
builder.Services.AddScoped<IInterfaceBMenuItems, clsMenuItemsRepositoryBridge>();
builder.Services.AddScoped<IReadableBMenuItems, BusinessLayerRestaurant.clsMenuItemsReader>();
builder.Services.AddScoped<IWritableBMenuItems, BusinessLayerRestaurant.clsMenuItemsWriter>();
builder.Services.AddScoped<IBusinessMenuItems, clsBusinessMenuItem>();

builder.Services.AddScoped<IWritableDOrders, DataLayerRestaurant.clsOrdersWriter>();
builder.Services.AddScoped<IReadableDOrders, DataLayerRestaurant.clsOrdersReader>();
builder.Services.AddScoped<IDataOrders, clsDataOrders>();
builder.Services.AddScoped<IWritableBOrders, BusinessLayerRestaurant.clsOrdersWriter>();
builder.Services.AddScoped<IReadableBOrders, BusinessLayerRestaurant.clsOrdersReader>();
builder.Services.AddScoped<IDTOBOrders, clsOrdersDtoContainer>();
builder.Services.AddScoped<IInterfaceBOrders, clsOrdersRepositoryBridge>();
builder.Services.AddScoped<ICompositionBOrders, clsStatusOrderLoader>();
builder.Services.AddScoped<ICompositionBOrders, clsEmployeeLoader>();
builder.Services.AddScoped<ICompositionBOrders, clsTableLoader>();
builder.Services.AddScoped<IBusinessOrders, clsBusinessOrders>();

builder.Services.AddScoped<IDataSettings, clsDataSettings>();
builder.Services.AddScoped<IWritableDSettings, DataLayerRestaurant.clsSettingsWriter>();
builder.Services.AddScoped<IReadableDSettings, DataLayerRestaurant.clsSettingsReader>();
builder.Services.AddScoped<IReadableBSettings, BusinessLayerRestaurant.clsSettingsReader>();
builder.Services.AddScoped<IWritableBSettings, BusinessLayerRestaurant.clsSettingsWriter>();
builder.Services.AddScoped<IDTOBSettings, clsSettingsDtoContainer>();
builder.Services.AddScoped<IInterfaceBSettings, clsSettingsRepositoryBridge>();
builder.Services.AddScoped<IBusinessSettings, clsBusinessSettings>();

builder.Services.AddScoped<IReadableDStatusMenus, DataLayerRestaurant.clsStatusMenusReader>();
builder.Services.AddScoped<IWritableDStatusMenus, DataLayerRestaurant.clsStatusMenusWriter>();
builder.Services.AddScoped<IDataStatusMenus, clsDataStatusMenus>();
builder.Services.AddScoped<IWritableBStatusMenus, BusinessLayerRestaurant.clsStatusMenusWriter>();
builder.Services.AddScoped<IReadableBStatusMenus, BusinessLayerRestaurant.clsStatusMenusReader>();
builder.Services.AddScoped<IDTOBStatusMenus, clsStatusMenusDtoContainer>();
builder.Services.AddScoped<IInterfaceBStatusMenus, clsStatusMenusRepositoryBridge>();
builder.Services.AddScoped<IBusinessStatusMenus, clsBusinessStatusMenus>();

builder.Services.AddScoped<IReadableDStatusOrders, DataLayerRestaurant.clsStatusOrdersReader>();
builder.Services.AddScoped<IWritableDStatusOrders, DataLayerRestaurant.clsStatusOrdersWriter>();
builder.Services.AddScoped<IDataStatusOrders, clsDataStatusOrders>();
builder.Services.AddScoped<IReadableBStatusOrders, BusinessLayerRestaurant.clsStatusOrdersReader>();
builder.Services.AddScoped<IWritableBStatusOrders, BusinessLayerRestaurant.clsStatusOrdersWriter>();
builder.Services.AddScoped<IDTOBStatusOrders, clsStatusOrdersDtoContainer>();
builder.Services.AddScoped<IInterfaceBStatusOrders, clsStatusOrdersRepositoryBridge>();
builder.Services.AddScoped<IBusinessStatusOrders, clsBusinessStatusOrders>();

builder.Services.AddScoped<IReadableDStatusTables, DataLayerRestaurant.clsStatusTablesReader>();
builder.Services.AddScoped<IWritableDStatusTables, DataLayerRestaurant.clsStatusTablesWriter>();
builder.Services.AddScoped<IDataStatusTables, clsDataStatusTables>();
builder.Services.AddScoped<IReadableBStatusTables, BusinessLayerRestaurant.clsStatusTablesReader>();
builder.Services.AddScoped<IWritableBStatusTables, BusinessLayerRestaurant.clsStatusTablesWriter>();
builder.Services.AddScoped<IDTOBStatusTables, clsStatusTablesDtoContainer>();
builder.Services.AddScoped<IInterfaceBStatusTables, clsStatusTablesRepositoryBridge>();
builder.Services.AddScoped<IBusinessStatusTables, clsBusinessStatusTables>();

builder.Services.AddScoped<IReadableDTables, DataLayerRestaurant.clsTablesReader>();
builder.Services.AddScoped<IWritableDTables, DataLayerRestaurant.clsTablesWriter>();
builder.Services.AddScoped<IDataTables, clsDataTables>();
builder.Services.AddScoped<IReadableBTables, BusinessLayerRestaurant.clsTablesReader>();
builder.Services.AddScoped<IWritableBTables, BusinessLayerRestaurant.clsTablesWriter>();
builder.Services.AddScoped<IDTOBTables, clsTablesDtoContainer>();
builder.Services.AddScoped<IInterfaceBTables, clsTablesRepositoryBridge>();
builder.Services.AddScoped<ICompositionBTables, clsStatusTableLoader>();
builder.Services.AddScoped<IBusinessTables, clsBusinessTables>();

builder.Services.AddScoped<IReadableDTypeItems, DataLayerRestaurant.clsTypeItemsReader>();
builder.Services.AddScoped<IWritableDTypeItems, DataLayerRestaurant.clsTypeItemsWriter>();
builder.Services.AddScoped<IDataTypeItems, clsDataTypeItems>();
builder.Services.AddScoped<IDTOBTypeItems, clsTypeItemsDtoContainer>();
builder.Services.AddScoped<IInterfaceBTypeItems, clsTypeItemsRepositoryBridge>();
builder.Services.AddScoped<IReadableBTypeItems, BusinessLayerRestaurant.clsTypeItemsReader>();
builder.Services.AddScoped<IWritableBTypeItems, BusinessLayerRestaurant.clsTypeItemsWriter>();
builder.Services.AddScoped<IBusinessTypeItems, clsBusinessTypeItems>();

builder.Services.AddScoped<IReadableDOrderDetails, DataLayerRestaurant.clsOrderDetailsReader>();
builder.Services.AddScoped<IWritableDOrderDetails, DataLayerRestaurant.clsOrderDetailsWriter>();
builder.Services.AddScoped<IDataOrderDetails, clsDataOrderDetails>();
builder.Services.AddScoped<IReadableBOrderDetails, BusinessLayerRestaurant.clsOrderDetailsReader>();
builder.Services.AddScoped<IWritableBOrderDetails, BusinessLayerRestaurant.clsOrderDetailsWriter>();
builder.Services.AddScoped<IDTOBOrderDetails, clsOrderDetailsDtoContainer>();
builder.Services.AddScoped<IInterfaceBOrderDetails, clsOrderDetailsRepositoryBridge>();
builder.Services.AddScoped<ICompositionBOrderDetails, clsOrderLoader>();
builder.Services.AddScoped<ICompositionBOrderDetails, clsMenuItemLoader>();
builder.Services.AddScoped<IBusinessOrderDetails, clsBusinessOrderDetails>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
