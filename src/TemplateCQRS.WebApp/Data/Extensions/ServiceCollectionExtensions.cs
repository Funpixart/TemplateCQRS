
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.Reflection;
using TemplateCQRS.Domain.Common;

namespace TemplateCQRS.WebApp.Data.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Dynamically adds all non-abstract classes from the specified or current namespace ending with "Service" to the service collection.
    ///     These classes must implement an interface that follows the convention "I[ClassName]".
    /// </summary>
    /// <param name="services">The IServiceCollection instance to extend.</param>
    public static void AddServicesFromAssembly(this IServiceCollection services)
    {
        var servicesTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            type is { IsClass: true, IsAbstract: false } 
            && type.Namespace?.EndsWith(".Services") == true 
            && type.Name.EndsWith("Service"));

        foreach (var serviceType in servicesTypes)
        {
            var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
            if (interfaceType == null)
            {
                services.AddScoped(serviceType);
                continue;
            }

            services.AddScoped(interfaceType, serviceType);
        }
    }

    public static void AddAuthenticationWithCookies(this IServiceCollection services)
    {
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
        });

        services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(Constants.AppPath))
            .SetApplicationName(Constants.AppCookies);

        services.ConfigureApplicationCookie(options => options.Cookie.Domain = Constants.Domain);
    }
}