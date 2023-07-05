namespace TemplateCQRS.Api.Configurations;

public class CachePolicy
{
    public string Name { get; private set; }
    public TimeSpan Expiration { get; private set; }
    public string Tag { get; private set; }
    public string[] VaryByQuery { get; private set; }

    private CachePolicy(string name, TimeSpan expiration, string tag, string[] varyByQuery = null)
    {
        Name = name;
        Expiration = expiration;
        Tag = tag;
        VaryByQuery = varyByQuery;
    }

    public static CachePolicy GetClaims => new ("getClaims", TimeSpan.FromDays(1), "claims");
    public static CachePolicy GetRoles => new ("getRoles", TimeSpan.FromDays(1), "roles");
    public static CachePolicy GetUsers => new ("getUsers", TimeSpan.FromDays(1), "users");
    public static CachePolicy GetUserBy => new ("getUserBy", TimeSpan.FromDays(1), "users", new[] { "id", "name", "email" });

    public static CachePolicy GetClaimsByRole => new("getClaimsByRole", TimeSpan.FromHours(1), "getClaimsByRole", new []{ "roleId" });
}
