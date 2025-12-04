namespace SimpleStorageSystem.Shared.Models;

public class JwtToken
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set;}
}