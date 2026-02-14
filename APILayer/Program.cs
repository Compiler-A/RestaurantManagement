using System.Data;
using BusinessLayerRestaurant;
using DataLayerRestaurant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataEmployee, clsDataEmployee>();
builder.Services.AddScoped<IBusinessEmployees, clsBusinessEmployees>();

builder.Services.AddScoped<IDataJobRoles, clsDataJobRoles>();
builder.Services.AddScoped<IBusinessJobRoles, clsBusinessJobRoles>();

builder.Services.AddScoped<IDataMenuItems, clsDataMenuItems>();
builder.Services.AddScoped<IBusinessMenuItems, clsBusinessMenuItem>();

builder.Services.AddScoped<IDataOrders, clsDataOrders>();
builder.Services.AddScoped<IBusinessOrders, clsBusinessOrders>();

builder.Services.AddScoped<IDataSettings, clsDataSettings>();
builder.Services.AddScoped<IBusinessSettings, clsBusinessSettings>();

builder.Services.AddScoped<IDataStatusMenus, clsDataStatusMenus>();
builder.Services.AddScoped<IBusinessStatusMenus, clsBusinessStatusMenus>();

builder.Services.AddScoped<IDataStatusOrders, clsDataStatusOrders>();
builder.Services.AddScoped<IBusinessStatusOrder, clsBusinessStatusOrders>();

builder.Services.AddScoped<IDataStatusTables, clsDataStatusTables>();
builder.Services.AddScoped<IBusinessStatusTables, clsBusinessStatusTables>();

builder.Services.AddScoped<IDataTables, clsDataTables>();
builder.Services.AddScoped<IDataTablesBusiness, clsBusinessTables>();

builder.Services.AddScoped<IDataTypeItems, clsDataTypeItems>();
builder.Services.AddScoped<IBusinessTypeItems, clsBusinessTypeItems>();

builder.Services.AddScoped<IDataOrderDetails, clsDataOrderDetails>();
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
