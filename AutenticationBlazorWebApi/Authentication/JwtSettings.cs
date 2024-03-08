namespace AutenticationBlazorWebApi.Server.Authentication;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }

    public string DurationInMinutes { get; set; }
    public string Key { get; set; }
}