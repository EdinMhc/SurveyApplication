using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Exceptions;
using Survey.API;
using Survey.API.Auhthorization.AnonymousUserHandler;
using Survey.API.Dapper;
using Survey.API.Global_Exception_Handler;
using Survey.Domain.Services.IdentityService.Options;
using Survey.Infrastructure;
using System.Text;
using System.Text.Json.Serialization;

// TODO
// Refactor the code.
// Create new authorization attribute with same roles.
// Update all survices
var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Logging.ClearProviders();

// Add services to the container.
ConfigureLogging();
builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionResponseHandlingFilter>();
}).AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<ContextClass>(
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration.GetConnectionString("ProjectDB")));

builder.Services.AddCors();

var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddDependencyInjections();
builder.Services.AddIdentityConfiguration();

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = true,
    ValidateLifetime = true,
    ValidAudience = jwtSettings.ValidAt,
    ValidIssuer = jwtSettings.Issuer,
};

builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddJwtAuthentication(tokenValidationParams);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddDapperDependency();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Users", policy =>
        policy.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy("IsAnonymousUser",
        policy => policy.AddRequirements(new AllowAnonymousAuthorizationRequirement()));
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionResponseHandlingFilter>();
})
    .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.ConfigureExceptionHandler();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
        // Filter out ASP.NET Core infrastructre logs that are Information and below
        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Information)
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}