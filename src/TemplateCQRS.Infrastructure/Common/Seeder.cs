using Microsoft.EntityFrameworkCore;
using TemplateCQRS.Domain.Common;
using TemplateCQRS.Domain.Models;

namespace TemplateCQRS.Infrastructure.Common;

public class Seeder
{
    public static void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Owner);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Admin);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Manager);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Supervisor);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Author);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Contributor);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.User);
        modelBuilder.Entity<Role>().HasData(Constants.DefaultRoles.Visitor);
    }
}