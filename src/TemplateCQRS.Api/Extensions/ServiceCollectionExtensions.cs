using System.Reflection;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using TemplateCQRS.Api.Configurations;
using TemplateCQRS.Api.Security;
using TemplateCQRS.Application.Common;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Infrastructure.Data;
using TemplateCQRS.Infrastructure.Repositories;
using System.Configuration;
using StackExchange.Redis;

namespace TemplateCQRS.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Extension method to add the UnitOfWork implementation to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance for which to add the UnitOfWork implementation.</param>
    public static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    /// <summary>
    ///     Extension method to add the generic Repository implementation to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance for which to add the generic Repository implementation.</param>
    public static void AddGenericRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    /// <summary>
    ///     Extension method to add a custom DbContext to the IServiceCollection with lazy loading proxies and MySQL configuration.
    /// </summary>
    /// <typeparam name="TContext">The type of the DbContext to add.</typeparam>
    /// <param name="services">The IServiceCollection instance for which to add the custom DbContext.</param>
    /// <param name="config">The IConfiguration instance used to configure the DbContext options.</param>
    public static void AddCustomDbContext<TContext>(this IServiceCollection services, IConfiguration config)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseMySql(GetMySqlCon(config), ServerVersion.AutoDetect(GetMySqlCon(config)));
        });
    }

    /// <summary>
    ///     Adds AutoMapper configuration and mappings to the service collection.
    ///     This extension method takes care of not overwriting non-null destination properties.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    public static void AddCustomAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.Internal()
                .ForAllMaps((map, exp) =>
                    exp.ForAllMembers(options =>
                        options.Condition(sourceMember => sourceMember != null)));
        }, typeof(Application.Program));
    }

    /// <summary>
    ///     Extension method to add SwaggerGen with custom options to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance for which to add the SwaggerGen with custom options.</param>
    public static void AddSwaggerGenWithOptions(this IServiceCollection services)
    {
        services.AddSwaggerGen(swag =>
        {
            swag.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API TemplateCQRS",
                Version = "v1",
                Description = "Desarrollado por https://funpixart.com / https://Funpixart.net",
            });

            swag.OperationFilter<CustomOperationFilter>();
        });
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }
    
    /// <summary>
    ///     Adds Output caching with predefined policies to the service collection.
    ///     It reads all the cache policies from the CachePolicy class and adds
    ///     them to the output cache options.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="config">The IConfiguration interface to access the configuration keys.</param>
    public static void AddRedisOutputCacheWithPolicy(this IServiceCollection services, IConfiguration config)
    {
        var redisSettings = config.GetSection("Redis");
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisSettings["Configuration"] ?? "localhost"));

        var cachePolicies = typeof(CachePolicy)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(pi => pi.PropertyType == typeof(CachePolicy))
            .Select(pi => pi.GetValue(null))
            .Cast<CachePolicy>();

        services.AddRedisOutputCache(options =>
        {
            foreach (var policy in cachePolicies)
            {
                if (policy?.VaryByQuery is null)
                {
                    options.AddPolicy(policy.Name, optionBuilder
                        => optionBuilder.Expire(policy.Expiration).Tag(policy.Tag));
                }
                else
                {
                    options.AddPolicy(policy.Name, optionBuilder
                        => optionBuilder.SetVaryByQuery(policy.VaryByQuery).Expire(policy.Expiration).Tag(policy.Tag));
                }
            }
        });
    }

    /// <summary>
    ///     Add custom identity with user, role and EF Stores from the context.
    /// </summary>
    /// <typeparam name="TUser">The user that inherits from <b>IdentityUser{TKey}</b><seealso cref="IdentityUser{TKey}" /></typeparam>
    /// <typeparam name="TRole">The role that inherits from <b>IdentityRoleTKey}</b><seealso cref="IdentityRole{TKey}" /></typeparam>
    /// <typeparam name="TContext">The custom context inherits from <b>DbContext</b><seealso cref="DbContext" /></typeparam>
    /// <typeparam name="TKey">The identify that is use for the Key type in the Identity.</typeparam>
    /// <param name="services">IServiceCollection</param>
    public static void AddCustomIdentity<TUser, TRole, TContext, TKey>(this IServiceCollection services)
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
    {
        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.FromMinutes(1);
        });

        services.AddIdentity<TUser, TRole>(options =>
        {
            // Default Lockout settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;

        }).AddRoles<TRole>().AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = Constants.AppCookies;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // CookieSecurePolicy.Always para produccion
                options.Cookie.SameSite = SameSiteMode.Strict;  // previene ataques CSRF
                options.AccessDeniedPath = "/accessdenied";
                options.LogoutPath = "/account/logout";
                options.LoginPath = "/login";
                options.SlidingExpiration = true;
                options.Cookie.MaxAge = TimeSpan.FromHours(12);
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
                options.Events.OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync;
            });
        services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(Constants.AppPath))
            .SetApplicationName(Constants.AppCookies);

        services.ConfigureApplicationCookie(options => options.Cookie.Domain = Constants.Domain);
    }

    /// <summary>
    ///     Extension method to add JWT-based authentication to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance for which to add the JWT-based authentication.</param>
    /// <param name="configuration">The IConfiguration instance used to configure the JWT settings.</param>
    public static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? string.Empty)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    /// <summary>
    ///     Retrieves MySQL connection string from the provided IConfiguration.
    /// </summary>
    /// <param name="config">The IConfiguration instance to extract connection details from.</param>
    /// <returns>A MySQL connection string built from the details stored in the configuration.</returns>
    private static string GetMySqlCon(IConfiguration config)
    {
        var server = config.GetConnectionString("MySQL_IP") ?? "";
        var port = config.GetConnectionString("MySQL_PORT") ?? "";
        var database = config.GetConnectionString("MySQL_DATABASE") ?? "";
        var user = config.GetConnectionString("MySQL_USER") ?? "";
        var password = config.GetConnectionString("MySQL_PASSWORD") ?? "";

        // Default port
        port = !string.IsNullOrEmpty(port) && port.Equals("3306") ? "" : $";Port={port}";

        return $"Server={server}{port};Database={database};User={user};Password={password};";
    }

    /// <summary>
    ///     Constructs a SQL Server connection string using the provided configuration.
    /// </summary>
    /// <param name="config">An IConfiguration instance for retrieving connection parameters.</param>
    /// <returns>The constructed SQL Server connection string.</returns>
    private static string GetSqlServerCon(IConfiguration config)
    {
        var server = config.GetConnectionString("MSSQL_IP") ?? "";
        var port = config.GetConnectionString("MSSQL_PORT") ?? "";
        var database = config.GetConnectionString("MSSQL_DATABASE") ?? "";
        var user = config.GetConnectionString("MSSQL_USER") ?? "";
        var password = config.GetConnectionString("MSSQL_PASSWORD") ?? "";
        var trustedConnection = config.GetConnectionString("TrustedConnection") ?? "";
        var persistSecurityInfo = config.GetConnectionString("PersistSecurityInfo") ?? "";
        var mars = config.GetConnectionString("MultipleActiveResultSets") ?? "";
        var encrypt = config.GetConnectionString("Encrypt") ?? "";
        var trustServerCertificate = config.GetConnectionString("TrustServerCertificate") ?? "";
        var connectionTimeout = config.GetConnectionString("ConnectionTimeout") ?? "";

        // Default port
        port = !string.IsNullOrEmpty(port) && port.Equals("1433") ? "" : $",{port}";

        var connectionString = $"Server={server}{port};Database={database};";
            
        if (!string.IsNullOrEmpty(server))
        {
            connectionString = $"Server=localhost;Initial Catalog={database};Trusted_Connection=True;TrustServerCertificate=True;";
            return connectionString;
        }

        if (!string.IsNullOrEmpty(user))
            connectionString += $"User Id={user};";

        if (!string.IsNullOrEmpty(password))
            connectionString += $"Password={password};";

        if (!string.IsNullOrEmpty(trustedConnection))
            connectionString += $"Trusted_Connection={trustedConnection};";

        if (!string.IsNullOrEmpty(persistSecurityInfo))
            connectionString += $"Persist Security Info={persistSecurityInfo};";

        if (!string.IsNullOrEmpty(mars))
            connectionString += $"MultipleActiveResultSets={mars};";

        if (!string.IsNullOrEmpty(encrypt))
            connectionString += $"Encrypt={encrypt};";

        if (!string.IsNullOrEmpty(trustServerCertificate))
            connectionString += $"TrustServerCertificate={trustServerCertificate};";

        if (!string.IsNullOrEmpty(connectionTimeout))
            connectionString += $"Connection Timeout={connectionTimeout};";

        return connectionString;
    }
}