using TemplateCQRS.Domain.Dto.User;

namespace TemplateCQRS.Api.Security;

public class UserTokenRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserTokenDto
{
    public string Token { get; set; }
    public InfoUserDto UserInfo { get; set; }
}