
namespace TemplateCQRS.Domain.Dto.User;

public class DetailUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public RoleDto Role { get; set; }
}