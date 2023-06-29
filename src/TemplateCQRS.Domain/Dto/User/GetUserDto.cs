using System.Net.Http;

namespace TemplateCQRS.Domain.Dto.User;

public class GetUserDto
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
