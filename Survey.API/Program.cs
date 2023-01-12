using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Survey.API.Dapper;
using Survey.API.Global_Exception_Handler;
using Survey.API.JwtRelated.Auhthorization.AnonymousUserHandler;
using Survey.Domain.Services.CompanyService;
using Survey.Domain.Services.IdentityService.Options;
using Survey.Infrastructure.ContextClass1;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Repositories;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

// TODO
// Refactor the code.
// Create new authorization attribute with same roles.
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

builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ContextClass>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<Survey.Domain.Services.SurveyService.ISurveyService, Survey.Domain.Services.SurveyService.SurveyService>();

builder.Services.AddScoped<Survey.Domain.Services.SurveyReport_Service.ISurveyReportService, Survey.Domain.Services.SurveyReport_Service.SurveyReportService>();

builder.Services.AddScoped<Survey.Domain.Services.QuestionService.IQuestionService, Survey.Domain.Services.QuestionService.QuestionService>();

builder.Services.AddScoped<Survey.Domain.Services.AnwserBlockService.IAnwserBlockService, Survey.Domain.Services.AnwserBlockService.AnwserBlockService>();

builder.Services.AddScoped<Survey.Domain.Services.AnwserService.IAnswerService, Survey.Domain.Services.AnwserService.AnswerService>();

builder.Services.AddScoped<Survey.Domain.Services.SurveyReportDataService.ISurveyReportDataService, Survey.Domain.Services.SurveyReportDataService.SurveyReportDataService>();

builder.Services.AddScoped<Survey.Domain.Services.IdentityService.Interfaces.IIdentityService, Survey.Domain.Services.IdentityService.IdentityService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<User, IdentityRole>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 8;
    o.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ContextClass>()
.AddDefaultTokenProviders();

var tokenValidationParams = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.SaveToken = true;
    x.TokenValidationParameters = tokenValidationParams;
});

builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
              builder =>
              {
                  builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build();
              });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer",
        },
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header,
      },
      new List<string>()
    },
  });
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddInfrastructure();

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

// app.ConfigureExceptionHandler();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

//app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Serilog Related
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
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
    };
}