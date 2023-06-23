namespace TemplateCQRS.Domain.Common;

public static class ApiRoutes
{
    public const string Root = "api/";
    public const string Version = "v1/";
    public const string Base = Root + Version;

    // Roles routes
    public const string Roles = Base + "roles";

    // Users routes
    public const string Users = Base + "users";
    public const string UsersChangePassword = Users + "/changepass";

    // Security
    public const string Security = Base + "security";
    public const string RequestToken = Security + "/requestToken";

    // Claims routes
    public const string Claim = Base + "claims";
}