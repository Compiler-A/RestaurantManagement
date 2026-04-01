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

builder.Services.AddScoped<IReadableDEmployees, clsReadableDEmployees>();
builder.Services.AddScoped<IWritableDEmployees, clsWritableDEmployees>();
builder.Services.AddScoped<IDataEmployees, clsDataEmployees>();
builder.Services.AddScoped<IReadableBEmployees, clsReadableBEmployees>();
builder.Services.AddScoped<IWritableBEmployees, clsWritableBEmployees>();
builder.Services.AddScoped<IDTOBEmployees, clsDTOBEmployees>();
builder.Services.AddScoped<IInterfaceBEmployees, clsInterfaceBEmployees>();
builder.Services.AddScoped<ICompositionBEmployees, clsJobRoleLoaderByEmployees>();
builder.Services.AddScoped<IBusinessEmployees, clsBusinessEmployees>();

builder.Services.AddScoped<IReadableDJobRoles, clsReadableDJobRoles>();
builder.Services.AddScoped<IWritableDJobRoles, clsWritableDJobRoles>();
builder.Services.AddScoped<IDataJobRoles, clsDataJobRoles>();
builder.Services.AddScoped<IWritableBJobRoles, clsWritableBJobRoles>();
builder.Services.AddScoped<IReadableBJobRoles, clsReadableBJobRoles>();
builder.Services.AddScoped<IInterfaceBJobRoles, clsInterfaceBJobRoles>();
builder.Services.AddScoped<IDTOBJobRoles, clsDTOBJobRoles>();
builder.Services.AddScoped<IBusinessJobRoles, clsBusinessJobRoles>();

builder.Services.AddScoped<IReadableDMenuItems, clsReadableDMenuItems>();
builder.Services.AddScoped<IWritableDMenuItems, clsWritableDMenuItems>();
builder.Services.AddScoped<IDataMenuItems, clsDataMenuItems>();
builder.Services.AddScoped<ICompositionBMenuItems, clsTypeItemLoaderByMenuItems>();
builder.Services.AddScoped<ICompositionBMenuItems, clsStatusMenuLoaderByMenuItems>();
builder.Services.AddScoped<IDTOBMenuItems, clsDTOBMenuItems>();
builder.Services.AddScoped<IInterfaceBMenuItems, clsInterfaceBMenuItems>();
builder.Services.AddScoped<IReadableBMenuItems, clsReadableBMenuItems>();
builder.Services.AddScoped<IWritableBMenuItems, clsWritableBMenuItem>();
builder.Services.AddScoped<IBusinessMenuItems, clsBusinessMenuItem>();

builder.Services.AddScoped<IWritableDOrders, clsWritableDOrders>();
builder.Services.AddScoped<IReadableDOrders, clsReadableDOrders>();
builder.Services.AddScoped<IDataOrders, clsDataOrders>();
builder.Services.AddScoped<IWritableBOrders, clsWritableBOrders>();
builder.Services.AddScoped<IReadableBOrders, clsReadableBOrders>();
builder.Services.AddScoped<IDTOBOrders, clsDTOBOrders>();
builder.Services.AddScoped<IInterfaceBOrders, clsInterfaceBOrders>();
builder.Services.AddScoped<ICompositionBOrders, clsStatusOrderLoaderByOrder>();
builder.Services.AddScoped<ICompositionBOrders, clsEmployeeLoaderByOrder>();
builder.Services.AddScoped<ICompositionBOrders, clsTableLoaderByOrder>();
builder.Services.AddScoped<IBusinessOrders, clsBusinessOrders>();

builder.Services.AddScoped<IDataSettings, clsDataSettings>();
builder.Services.AddScoped<IWritableDSettings, clsWritableDSettings>();
builder.Services.AddScoped<IReadableDSettings, clsReadableDSettings>();
builder.Services.AddScoped<IReadableBSettings, clsReadableBSettings>();
builder.Services.AddScoped<IWritableBSettings, clsWritableBSettings>();
builder.Services.AddScoped<IDTOBSettings, clsDTOBSettings>();
builder.Services.AddScoped<IInterfaceBSettings, clsInterfaceBSettings>();
builder.Services.AddScoped<IBusinessSettings, clsBusinessSettings>();

builder.Services.AddScoped<IReadableDStatusMenus, clsReadableDStatusMenus>();
builder.Services.AddScoped<IWritableDStatusMenus,clsWritableDStatusMenus>();
builder.Services.AddScoped<IDataStatusMenus, clsDataStatusMenus>();
builder.Services.AddScoped<IWritableBStatusMenus, clsWritableBStatusMenus>();
builder.Services.AddScoped<IReadableBStatusMenus, clsReadableBStatusMenus>();
builder.Services.AddScoped<IDTOBStatusMenus, clsDTOBStatusMenus>();
builder.Services.AddScoped<IInterfaceBStatusMenus, clsInterfaceBStatusMenus>();
builder.Services.AddScoped<IBusinessStatusMenus, clsBusinessStatusMenus>();

builder.Services.AddScoped<IReadableDStatusOrders, clsReadableDStatusOrders>();
builder.Services.AddScoped<IWritableDStatusOrders, clsWritableDStatusOrders>();
builder.Services.AddScoped<IDataStatusOrders, clsDataStatusOrders>();
builder.Services.AddScoped<IReadableBStatusOrders, clsReadableBStatusOrders>();
builder.Services.AddScoped<IWritableBStatusOrders, clsWritableBStatusOrders>();
builder.Services.AddScoped<IDTOBStatusOrders, clsDTOBStatusOrders>();
builder.Services.AddScoped<IInterfaceBStatusOrders, clsInterfaceBStatusOrders>();
builder.Services.AddScoped<IBusinessStatusOrders, clsBusinessStatusOrders>();

builder.Services.AddScoped<IReadableDStatusTables, clsReadableDStatusTables>();
builder.Services.AddScoped<IWritableDStatusTables, clsWritableDStatusTables>();
builder.Services.AddScoped<IDataStatusTables, clsDataStatusTables>();
builder.Services.AddScoped<IReadableBStatusTables, clsReadableBStatusTables>();
builder.Services.AddScoped<IWritableBStatusTables, clsWritableBStatusTables>();
builder.Services.AddScoped<IDTOBStatusTables, clsDTOBStatusTables>();
builder.Services.AddScoped<IInterfaceBStatusTables, clsInterfaceBStatusTables>();
builder.Services.AddScoped<IBusinessStatusTables, clsBusinessStatusTables>();

builder.Services.AddScoped<IReadableDTables, clsReadableDTables>();
builder.Services.AddScoped<IWritableDTables, clsWritableDTables>();
builder.Services.AddScoped<IDataTables, clsDataTables>();
builder.Services.AddScoped<IReadableBTables, clsReadableBTables>();
builder.Services.AddScoped<IWritableBTables, clsWritableBTables>();
builder.Services.AddScoped<IDTOBTables, clsDTOBTables>();
builder.Services.AddScoped<IInterfaceBTables, clsInterfaceBTables>();
builder.Services.AddScoped<ICompositionBTables, clsStatusTableLoaderByTables>();
builder.Services.AddScoped<IBusinessTables, clsBusinessTables>();

builder.Services.AddScoped<IReadableDTypeItems, clsReadableDTypeItems>();
builder.Services.AddScoped<IWritableDTypeItems, clsWritableDTypeItems>();
builder.Services.AddScoped<IDataTypeItems, clsDataTypeItems>();
builder.Services.AddScoped<IDTOBTypeItems, clsDTOBTypeItems>();
builder.Services.AddScoped<IInterfaceBTypeItems, clsInterfaceBTypeItems>();
builder.Services.AddScoped<IReadableBTypeItems, clsReadableBTypeItems>();
builder.Services.AddScoped<IWritableBTypeItems, clsWritableBTypeItems>();
builder.Services.AddScoped<IBusinessTypeItems, clsBusinessTypeItems>();

builder.Services.AddScoped<IReadableDOrderDetails, clsReadableDOrderDetails>();
builder.Services.AddScoped<IWritableDOrderDetails, clsWritableDOrderDetails>();
builder.Services.AddScoped<IDataOrderDetails, clsDataOrderDetails>();
builder.Services.AddScoped<IReadableBOrderDetails, clsReadableBOrderDetails>();
builder.Services.AddScoped<IWritableBOrderDetails, clsWritableBOrderDetails>();
builder.Services.AddScoped<IDTOBOrderDetails, clsDTOBOrderDetails>();
builder.Services.AddScoped<IInterfaceBOrderDetails, clsInterfaceBOrderDetails>();
builder.Services.AddScoped<ICompositionBOrderDetails, clsOrderLoaderByOrderDetails>();
builder.Services.AddScoped<ICompositionBOrderDetails, clsMenuItemLoaderOrderDetails>();
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
