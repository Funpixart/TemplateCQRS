using System.Text.Json;

namespace TemplateCQRS.Api.Security;

public class TokenGenerationRequest
{
    public string Email { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public Dictionary<string, JsonElement> CustomClaims { get; set; } = new ();
}