namespace TemplateCQRS.Domain.Dto.Role;

public class InfoRoleClaimDto
{
    public string Name { get; set; }
    public List<string?> Claims { get; set; }
}