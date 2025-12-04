namespace SimpleStorageSystem.Shared.Models;

public class Session
{
    public AccessToken? AccessToken { get; set; }
    private string? refreshToken;

    public string? RefreshToken
    {
        get => refreshToken;
        set => refreshToken = value;
    }

}
