using APILayer.Authorization;
using APILayer.Authorization.Employee;
using APILayer.Authorization.Order;
using APILayer.Extensions;
using APILayer.Filters;
using APILayer.Middleware;
using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters define how incoming JWTs will be validated.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "RMAPI",
            ValidAudience = "RMAPIEmployees",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_RM123456"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeOwnerOrAdmin", policy =>
        policy.Requirements.Add(new EmployeeOwnerOrAdminRequirement()));
    options.AddPolicy("EmployeeByUserNameOwnerOrAdmin", policy =>
        policy.Requirements.Add(new EmployeeUserNameOwnerOrAdminRequirement()));
    options.AddPolicy("WaiterOwnerOrAdmin", policy =>
        policy.Requirements.Add(new WaiterOwnerOrAdminRequirement()));
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
    options.AddPolicy("GetAllLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
    options.AddPolicy("GetOneLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
    options.AddPolicy("AddLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
    options.AddPolicy("UpdateLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 15,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
    options.AddPolicy("DeleteLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    options.OperationFilter<DefaultResponsesOperationFilter>();
});

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

builder.Services.AddLoginServices();
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
builder.Services.AddHashingServices();

builder.Services.AddSingleton<IAuthorizationHandler, EmployeeUserNameOwnerOrAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, EmployeeOwnerOrAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, WaiterOwnerOrAdminHandler>();
builder.Services.AddSingleton<IMyLogger, clsMyLogger>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("RMApiCorsPolicy", policy =>
    {
        policy.WithOrigins(
            "https://localhost:7292",
            "http://localhost:5223"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
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


app.UseRateLimiter();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many login attempts. Please try again later.");
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
