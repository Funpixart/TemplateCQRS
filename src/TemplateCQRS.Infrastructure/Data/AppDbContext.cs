using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TemplateCQRS.Domain.Models;
using TemplateCQRS.Infrastructure.Common;

namespace TemplateCQRS.Infrastructure.Data;

public partial class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Seeder.SeedRoles(modelBuilder);
    }
}