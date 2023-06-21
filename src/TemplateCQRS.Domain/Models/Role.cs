using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TemplateCQRS.Domain.Models;

public class Role : IdentityRole<Guid>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id { get; set; } = Guid.NewGuid();

    [Required][MaxLength(50)] public override string Name { get; set; } = string.Empty;

    [Required][MaxLength(150)] public string Description { get; set; } = string.Empty;

    [Required] public int AccessLevel { get; set; } = 1;

    public bool IsSystemRole { get; set; }
}