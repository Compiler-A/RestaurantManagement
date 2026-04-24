using APILayer.Authorization;
using APILayer.Authorization.Employee;
using APILayer.Authorization.Order;
using APILayer.Extensions.Configuration;
using APILayer.Extensions.Security;
using APILayer.Extensions.Services;
using APILayer.Middleware;
using BusinessLayerRestaurant.Classes;
using DataLayerRestaurant;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMySettingsConfiguration(builder.Configuration);
builder.Services.AddJwtSettingConfiguration(builder.Configuration);

builder.Services.AddAuthenticationExtension();
builder.Services.AddAuthorizationExtension();
builder.Services.AddRateLimitingExtension();


builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenExtension();


builder.Services.AddServicesExtensions();

builder.Services.AddCorsSecurityExtension();


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
app.UseCors("RMApiCorsPolicy");

app.UseRateLimitingExtension();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
