using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using TemplateCQRS.Domain.Dto.User;
using TemplateCQRS.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Application.Common;
using TemplateCQRS.WebApp.Data.Common;

namespace TemplateCQRS.WebApp.Data.Services;

public class SecurityService : BaseService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;

    public ClaimsPrincipal? Principal { get; set; }
    public User? User { get; set; }

    public event Action? Authenticated;

    public SecurityService(AuthenticationStateProvider authenticationStateProvider, 
        IHttpContextAccessor httpContextAccessor, 
        HttpClient httpClient, IConfiguration configuration) : base(configuration)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
    }

    public async Task<bool> InitializeAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        Principal = authenticationState.User;

        var result = IsAuthenticated();
        if (result)
        {
            Authenticated?.Invoke();
        }

        return result;
    }

    public bool IsAuthenticated() => Principal?.Identity is not null && (Principal is not null && Principal.Identity.IsAuthenticated);

    public async Task<Payload<bool, string>> AuthenticateAsync(UserTokenRequest requestData)
    {
        // Request token and expect response
        var response = await _httpClient.PostAsJsonAsync(ApiUrl + ApiRoutes.RequestToken, requestData);

        if (response.IsSuccessStatusCode)
        {
            // Read and store the content as the User Token Dto
            var tokenResponse = await response.Content.ReadFromJsonAsync<UserTokenDto>();
            if (tokenResponse is not null)
            {
                // Read and store the jwt token and retrieve the information.
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(tokenResponse.Token);

                // TODO Validate Issuer and Audience here.

                // Check expiration
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    // TODO do something if the token expire.
                    return "Token expired!";
                }

                // TODO verify what other claims should be inserted
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, tokenResponse.UserInfo.Name),
                    new (ClaimTypes.Email, tokenResponse.UserInfo.Email)
                };

                claims.AddRange(tokenResponse.UserInfo.RolesDto.Select(role => new Claim(ClaimTypes.Role, role.Name)));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Once the user is verified and the claims is created, sign in the user via cookies.
                // Keep in mind this needs to be executed via Razor pages or static pages since the Cookie is not started
                if (_httpContextAccessor.HttpContext is not null)
                {
                    await _httpContextAccessor.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                    return true;
                }
            }
        }

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonResponse>();
        return $"[{response.StatusCode}] Response: {jsonResponse.Message}";
    }
}

public class BaseService
{
    private readonly IConfiguration _configuration;

    public string ApiUrl { get; set; }

    public BaseService(IConfiguration configuration)
    {
        _configuration = configuration;
        ApiUrl = _configuration["ApiUrl"] ?? "http://localhost:5034/";
    }
}