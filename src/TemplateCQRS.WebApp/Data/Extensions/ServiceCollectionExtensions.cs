
using TemplateCQRS.Domain.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.Reflection;

namespace TemplateCQRS.WebApp.Data.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Dynamically adds all non-abstract classes from the specified or current namespace ending with "Service" to the service collection.
    ///     These classes must implement an interface that follows the convention "I[ClassName]".
    /// </summary>
    /// <param name="services">The IServiceCollection instance to extend.</param>
    /// <param name="namespace">The namespace to scan for services. If not provided or empty, the current application's friendly name will be used.</param>
    public static void AddServicesFromAssembly(this IServiceCollection services, string @namespace = "")
    {
        @namespace = string.IsNullOrEmpty(@namespace) ? AppDomain.CurrentDomain.FriendlyName : @namespace;

        var servicesTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            type is { IsClass: true, IsAbstract: false }
            && type.Namespace == @namespace + ".Services"
            && type.Name.EndsWith("Service"));

        foreach (var serviceType in servicesTypes)
        {
            var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
            if (interfaceType == null)
            {
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