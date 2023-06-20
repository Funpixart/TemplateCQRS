
namespace TemplateCQRS.Domain.Dto.Role;

public class CreateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int AccessLevel { get; set; }
}