using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateCQRS.Domain.Models
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

    }

    public class UserRole : IdentityUserRole<Guid> { }

    public class UserLogin : IdentityUserLogin<Guid> { }

    public class UserToken : IdentityUserToken<Guid> { }

    public class UserClaim : IdentityUserClaim<Guid> { }
}
