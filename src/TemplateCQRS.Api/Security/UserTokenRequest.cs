namespace TemplateCQRS.Api.Security;

public class UserTokenRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}