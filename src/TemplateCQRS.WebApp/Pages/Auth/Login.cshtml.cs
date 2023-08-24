using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TemplateCQRS.Domain.Dto.User;
using TemplateCQRS.WebApp.Data.Services;

namespace TemplateCQRS.WebApp.Pages.Auth;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SecurityService _securityService;

    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    [TempData] public string Message { get; set; } = "";
    
    public LoginModel(SecurityService securityService)
    {
        _securityService = securityService;
    }
    
    public IActionResult OnGet()
    {
        if (HttpContext.User.Identity is { IsAuthenticated: true })
            return LocalRedirect("/");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = "~/")
    {
        var requestData = new UserTokenRequest
        {
            Email = Username,
            Password = Password
        };

        var payload = await _securityService.AuthenticateAsync(requestData);
        if (payload.IsSuccess)
        {
            return LocalRedirect(returnUrl);
        }

        Message = payload.Error;
        return Page();
    }
}