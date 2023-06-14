using TemplateCQRS.Domain.Models;

namespace TemplateCQRS.Api.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        app.MapGet("/role-get", Get).RequireAuthorization();
        app.MapPost("/role-post", Post).RequireAuthorization();
        app.MapPut("/role-put", Put).RequireAuthorization();
        app.MapDelete("/role-delete", Delete).RequireAuthorization();
    }

    public static Task<IResult> Get()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> Post()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> Put()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> Delete()
    {
        throw new NotImplementedException();
    }
}