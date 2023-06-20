namespace TemplateCQRS.Domain.Dto.User;

public class UpdateUserDto
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateUserPasswordDto
{
    public Guid Id { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}