using HealthCare.Configuration;
using HealthCare.Dtos;
using HealthCare.Mapping;
using HealthCare.Models;
using HealthCare.Repositories;
using HealthCare.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");

builder.Services.AddDbContext<HealthcaredbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Register repositories
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>(); // Example for other repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();

// Register AutoMapper with assembly scanning
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure JWT authentication
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddTransient<TokenService>();

builder.Services.Configure<WhatsAppSettings>(builder.Configuration.GetSection("WhatsAppSettings"));
builder.Services.AddSingleton<MessageService>();

// Add Identity services
builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<HealthcaredbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints(); // Ensure other endpoints are available

// Add JWT Bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var authSettings = builder.Configuration.GetSection("JwtSettings").Get<AuthSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = authSettings.Issuer,
        ValidAudience = authSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            logger.LogError("Authentication failed: {ExceptionMessage}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated: {Token}", context.SecurityToken);
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token received: {Token}", context.Token);
            return Task.CompletedTask;
        }
    };
});

// Logging 
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

builder.Logging.ClearProviders().AddConsole().SetMinimumLevel(LogLevel.Information);

// Swagger configuration with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "JwtAuthDemo", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
