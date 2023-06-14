using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TemplateCQRS.Domain.Models;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Today;

    [Required] public DateTime LastLogin { get; set; } = DateTime.Today;

    [Required] public override string SecurityStamp { get; set; } = Guid.NewGuid().ToString("D");

    [Required] public bool IsActive { get; set; } = true;
}