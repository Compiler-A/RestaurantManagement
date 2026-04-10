using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using APILayer.Extensions;
using APILayer.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Configure strongly typed settings objects
builder.Services.Configure<clsMySettings>(
    builder.Configuration.GetSection("MySettings"));

builder.Services.AddEmployeesServices();
builder.Services.AddJobRolesServices();
builder.Services.AddMenuItemsServices();
builder.Services.AddOrderDetailsServices();
builder.Services.AddOrdersServices();
builder.Services.AddSettingsServices();
builder.Services.AddStatusMenusServices();
builder.Services.AddStatusOrdersServices();
builder.Services.AddStatusTablesServices();
builder.Services.AddTablesServices();
builder.Services.AddTypeItemsServices();

builder.Services.AddSingleton<IMyLogger, clsMyLogger>();


var app = builder.Build();

//Middleware for global exception handling
app.UseMiddleware<GlobalExceptionMiddleware>();


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
