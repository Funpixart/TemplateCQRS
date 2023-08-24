using TemplateCQRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateCQRS.UnitTest.Data;

public class MockingData
{
    public static readonly Guid[] UserIds =
    {
        Guid.Parse("d3bfa3e2-4dce-4d33-a0c1-8f45a9e6f749"),
        Guid.Parse("e482b26f-06b6-4547-b424-0808538f8bfe"),
        Guid.Parse("c33c206a-4f20-44c5-a2b1-8c98c5aa8d6a"),
        Guid.Parse("e2e86f84-4dce-456b-9545-00a1b34f5b7b"),
        Guid.Parse("64f84c4a-06b6-4f47-9742-081f8a9f8b5e")
    };

    public static readonly Guid[] RoleIds =
    {
        Guid.Parse("ae68a3d3-4358-4c77-b461-0985a0f8b7b9"),
        Guid.Parse("3f3c4e4a-56ce-4233-8341-0a68a6f8c5e0"),
        Guid.Parse("b96137fa-54d1-4e4a-8142-0a89c6e6f7e1"),
        Guid.Parse("04d96a69-57b1-4e48-8c47-0889c6f8c7b2"),
        Guid.Parse("70f94c3a-5a3e-4345-9443-0c89c7f8c7b3")
    };

    public static readonly int[] RoleClaimIds =
    {
        1, 2, 3, 4, 5
    };

    public static List<User> GenerateUsers()
    {
        return new List<User>
        {
            new User
            {
                Id = UserIds[0],
                CreatedAt = DateTime.Today,
                LastLogin = DateTime.Today,
                IsActive = true
            },
            new User
            {
                Id = UserIds[1],
                CreatedAt = DateTime.Today.AddDays(-1),
                LastLogin = DateTime.Today,
                IsActive = true
            },
            new User
            {
                Id = UserIds[2],
                CreatedAt = DateTime.Today.AddDays(-2),
                LastLogin = DateTime.Today,
                IsActive = false
            },
            new User
            {
                Id = UserIds[3],
                CreatedAt = DateTime.Today.AddDays(-3),
                LastLogin = DateTime.Today,
                IsActive = false
            },
            new User
            {
                Id = UserIds[4],
                CreatedAt = DateTime.Today.AddDays(-4),
                LastLogin = DateTime.Today,
                IsActive = true
            }
        };
    }

    public static List<Role> GenerateRoles()
    {
        return new List<Role>
        {
            new Role
            {
                Id = RoleIds[0],
                Name = "Admin",
                Description = "In a single app this role will have all permissions and can manage all other roles.",
                AccessLevel = 7,
                IsSystemRole = true
            },
            new Role
            {
                Id = RoleIds[1],
                Name = "User",
                Description = "This role has regular user permissions.",
                AccessLevel = 3,
                IsSystemRole = false
            },
            new Role
            {
                Id = RoleIds[2],
                Name = "Guest",
                Description = "This role has limited permissions.",
                AccessLevel = 1,
                IsSystemRole = false
            },
            new Role
            {
                Id = RoleIds[3],
                Name = "Editor",
                Description = "This role can edit content.",
                AccessLevel = 5,
                IsSystemRole = false
            },
            new Role
            {
                Id = RoleIds[4],
                Name = "Contributor",
                Description = "This role can contribute content but not edit.",
                AccessLevel = 4,
                IsSystemRole = false
            },
        };
    }

    public static List<UserRole> GenerateUsersRoles()
    {
        return new List<UserRole>
        {
            new UserRole
            {
                RoleId = RoleIds[0],
                UserId = UserIds[0]
            },
            new UserRole
            {
                RoleId = RoleIds[1],
                UserId = UserIds[1]
            },
            new UserRole
            {
                RoleId = RoleIds[2],
                UserId = UserIds[2]
            },
            new UserRole
            {
                RoleId = RoleIds[3],
                UserId = UserIds[3]
            },
            new UserRole
            {
                RoleId = RoleIds[4],
                UserId = UserIds[4]
            }
        };
    }

    public static List<RoleClaim> GenerateRoleClaims()
    {
        var roleClaims = new List<RoleClaim>();

        for (int i = 0; i < RoleIds.Length; i++)
        {
            for (int j = 1; j <= 2; j++)
            {
                roleClaims.Add(new RoleClaim
                {
                    Id = RoleClaimIds[i] * 10 + j, // This ensures unique IDs for each RoleClaim
                    RoleId = RoleIds[i],
                    ClaimType = $"claimType{i}_{j}",
                    ClaimValue = j % 2 == 0 ? "true" : "false"  // alternate between true and false
            });
    }
}

        return roleClaims;
    }
}