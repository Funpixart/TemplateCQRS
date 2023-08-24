using Serilog;
using TemplateCQRS.WebApp.Components;
using TemplateCQRS.WebApp.Data.Extensions;
using TemplateCQRS.WebApp.Data.Middleware;
using TemplateCQRS.WebApp.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Set up logging
builder.Host.UseSerilog(SerilogExtensions.InitializeSerilog(config));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddFunpixartServices();
builder.Services.AddAuthenticationWithCookies();
builder.Services.AddHttpClient();

// Add all services in this assembly
builder.Services.AddServicesFromAssembly();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Set up Serilog request logging
app.UseSerilogRequestLogging();

// Set up authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseExceptionCatcherMiddleware();
app.MapAccountEndpoints();

app.Run();