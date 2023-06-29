namespace TemplateCQRS.Domain.Dto.Role;

public class InfoRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int AccessLevel { get; set; }
    public bool IsSystemRole { get; set; }
    public List<string?> Claims { get; set; }
}