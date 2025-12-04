using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using PresenceProject.Converters;
using PresenceProject.Core;
using PresenceProject.Core.Interfaces;
using PresenceProject.Data;
using PresenceProject.Data.Repositories;
using PresenceProject.Converters;  // ? כאן מייבאים את הקונברטרים שלך
using PresenceProject.Middlewares;
using PresenceProject.Service.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// הגדרת NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Enum כמחרוזת
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());     // קונברטר ל-DateOnly
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());     // קונברטר ל-TimeOnly
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            new List<string>()
        }
    });
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// EF Core
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

try
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Middlewares
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<TokenMiddleware>();
    app.UseMiddleware<ShabbatMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    logger.LogInformation("Application is starting...");
    app.Run();
}
catch (Exception ex)
{
    logger.LogError(ex, "Application encountered a problem");
    throw;
}




