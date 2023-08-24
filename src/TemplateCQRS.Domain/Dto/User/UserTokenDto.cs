using TemplateCQRS.Domain.Dto.User;

namespace TemplateCQRS.Domain.Dto.User;

public class UserTokenDto
{
    public string Token { get; set; }
    public InfoUserDto UserInfo { get; set; }
}