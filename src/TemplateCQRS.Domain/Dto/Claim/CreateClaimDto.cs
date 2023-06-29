namespace TemplateCQRS.Domain.Dto.Claim;

public class CreateClaimDto
{
    public Guid RoleId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}
