using FluentValidation;
using Serilog;
using TemplateCQRS.Api.Extensions;
using TemplateCQRS.Api.Middleware;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Models;
using TemplateCQRS.Infrastructure.Data;

namespace TemplateCQRS.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        // Set up logging
        builder.Host.UseSerilog(SerilogExtensions.InitializeSerilog(config));

        // Leave string empty to use the launchSetting.json urls.
        builder.WebHost.UseUrls(config["UseUrls"] ?? "");

        // Register controllers and API endpoints
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Set up Swagger documentation
        builder.Services.AddSwaggerGenWithOptions();

        // DbContext and Identity
        builder.Services.AddCustomDbContext<AppDbContext>(config);
        builder.Services.AddCustomIdentity<User, Role, AppDbContext, Guid>();
        builder.Services.AddAuthenticationWithJwt(config);
        builder.Services.AddAuthorization();

        // Add custom services
        builder.Services.AddUnitOfWork();
        builder.Services.AddGenericRepository();

        // Add OutputCache with policies
        builder.Services.AddRedisOutputCacheWithPolicy(config);

        // Validators
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(Application.Program));

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Application.Program));

        // Set up MediatR for command and query handling
        builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Application.Program).Assembly));

        // Build the application
        var app = builder.Build();

        // Middleware to catch exceptions
        app.UseExceptionCatcherMiddleware();

        // Set up Serilog request logging
        app.UseSerilogRequestLogging();

        // Apply any pending migrations
        app.ApplyMigrations<AppDbContext>();

        // Set up Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI();

        // Use the OutputCache
        app.UseOutputCache();

        // Authorization & Authentication
        app.UseAuthentication();
        app.UseAuthorization();

        // Map application endpoints and controllers
        app.MapApplicationEndpoints();
        app.MapControllers();

        app.Run();
    }
}