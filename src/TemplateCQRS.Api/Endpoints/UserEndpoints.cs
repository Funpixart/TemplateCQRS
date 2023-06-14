namespace TemplateCQRS.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        // TODO: Add this into a constant. eg. "api/v1/user"
        app.MapGet("/user-get", Get).RequireAuthorization();
        app.MapPost("/user-post", Post).RequireAuthorization();
        app.MapPut("/user-put", Put).RequireAuthorization();
        app.MapDelete("/user-delete", Delete).RequireAuthorization();
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