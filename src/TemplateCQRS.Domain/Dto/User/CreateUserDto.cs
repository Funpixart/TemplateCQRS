namespace TemplateCQRS.Domain.Dto.User;

public class CreateUserDto
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsActive { get; set; }
}