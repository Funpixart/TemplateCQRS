using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateCQRS.Domain.Dto.Role;

public class RoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int AccessLevel { get; set; }
    public bool IsSystemRole { get; set; }
}