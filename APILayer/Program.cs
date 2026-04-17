using APILayer.Extensions;
using APILayer.Middleware;
using BusinessLayerRestaurant.Classes;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
