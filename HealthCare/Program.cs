using HealthCare.Configuration;
using HealthCare.Dtos;
using HealthCare.Mapping;
using HealthCare.Models;
using HealthCare.Repositories;
using HealthCare.Services;
using HealthCare.Logging;  
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
using HealthCare.Middlewares;
using Serilog;
using HealthCare.Exceptions;
using SoapCore;
using Refit;
using Serilog.Extensions.Logging;

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
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>(); 
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();

// Register AutoMapper with assembly scanning
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Sink(new EFCoreSink(builder.Services.BuildServiceProvider()))
    .CreateLogger();

builder.Host.UseSerilog();


// Configure JWT authentication
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddHostedService<StartupRolesInitializer>();

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
            Log.Error("Authentication failed: {ExceptionMessage}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Log.Information("Token validated: {Token}", context.SecurityToken);
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Log.Information("Token received: {Token}", context.Token);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            if (context.AuthenticateFailure != null)
            {
                Log.Error("Authentication failed during challenge: {ExceptionMessage}", context.AuthenticateFailure.Message);
            }
            else
            {
                Log.Warning("Unauthorized access attempt: {Scheme}", context.Scheme.Name);
            }
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            var message = "You do not have  access to this resource.";
            Log.Warning("Forbidden access attempt to: {Path}, Message : {message}", context.HttpContext.Request.Path,message);
            return Task.CompletedTask;
        }
    };
});



// Swagger configuration with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Basic .NET Health Care Application System", Version = "v1" });
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

// Refit Configuration

var whatsappSettings = builder.Configuration.GetSection("WhatsAppSettings").Get<WhatsAppSettings>();

var refitSettings = new RefitSettings

{
    AuthorizationHeaderValueGetter = (rq, rc) => Task.FromResult(whatsappSettings.AccessToken),
   HttpMessageHandlerFactory = () => new HttpClientHandler
   {
       ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
   }
};
builder.Services.AddTransient<SerilogLoggingHandler>();
builder.Services
       .AddRefitClient<IWhatsAppApi>(refitSettings)
       .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://graph.facebook.com"))
         .AddHttpMessageHandler<SerilogLoggingHandler>();

// SOAP CONIFGURATION
builder.Services.AddSoapCore();
builder.Services.AddScoped<ISoapService, SoapService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthorizationLoggingMiddleware>();
//app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();



((IApplicationBuilder)app).UseSoapEndpoint<ISoapService>("/service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
app.MapControllers();

app.Run();
