namespace SimpleStorageSystem.WebAPI.Models;

public class Session
{
    public string? AccessToken { get; set; }
    public DateTime? Expiration { get; set; }
    public string? RefreshToken { get; set; }
}
