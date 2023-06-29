namespace TemplateCQRS.Domain.Dto.Claim;

public class InfoClaimDto
{
    public int Id { get; set; }
    public Guid RoleId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}